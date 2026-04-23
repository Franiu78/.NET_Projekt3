using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Task3;

public static class ImageFilters
{
    // Zamienia obraz na odcienie szarości, używając klasycznej ważonej średniej RGB.
    public static WriteableBitmap Grayscale(WriteableBitmap source) => Transform(source, static (r, g, b, a) =>
    {
        var gray = (byte)Math.Round(0.299F * r + 0.587F * g + 0.114F * b);
        return (gray, gray, gray, a);
    });

    // Odwraca kolory obrazu, czyli tworzy negatyw.
    public static WriteableBitmap Negative(WriteableBitmap source) => Transform(source, static (r, g, b, a) =>
        ((byte)(255 - r), (byte)(255 - g), (byte)(255 - b), a));

    // Zamienia obraz na czarno-biały z prostym progiem jasności.
    public static WriteableBitmap Threshold(WriteableBitmap source) => Transform(source, static (r, g, b, a) =>
    {
        var gray = 0.299F * r + 0.587F * g + 0.114F * b;
        var value = gray > 128 ? (byte)255 : (byte)0;
        return (value, value, value, a);
    });

    // Odbija obraz poziomo, zamieniając miejscami piksele w każdym wierszu.
    public static WriteableBitmap MirrorHorizontal(WriteableBitmap source)
    {
        using var frame = source.Lock();
        var width = frame.Size.Width;
        var height = frame.Size.Height;
        var rowBytes = frame.RowBytes;
        // Bierzemy rozmiar piksela z aktualnego formatu, bo obraz może mieć 3 albo 4 bajty na piksel.
        var bytesPerPixel = frame.Format.BitsPerPixel / 8;
        var formatName = frame.Format.ToString();
        var buffer = ReadBuffer(frame.Address, rowBytes * height);
        var output = new byte[buffer.Length];

        // Kopiujemy dane po wierszach, a potem odwracamy kolejność pikseli w każdym wierszu.
        for (var y = 0; y < height; y++)
        {
            var rowStart = y * rowBytes;

            for (var x = 0; x < width; x++)
            {
                var sourceOffset = rowStart + (width - 1 - x) * bytesPerPixel;
                var targetOffset = rowStart + x * bytesPerPixel;
                var (r, g, b, a) = ReadPixel(buffer, sourceOffset, formatName);
                WritePixel(output, targetOffset, r, g, b, a);
            }
        }

        return CreateBitmap(source.PixelSize, output);
    }

    // Wspólny helper dla filtrów zmieniających kolor piksela, żeby nie powielać logiki odczytu i zapisu.
    private static WriteableBitmap Transform(WriteableBitmap source, Func<byte, byte, byte, byte, (byte r, byte g, byte b, byte a)> transform)
    {
        using var frame = source.Lock();
        var width = frame.Size.Width;
        var height = frame.Size.Height;
        var rowBytes = frame.RowBytes;
        // Jednolity rozmiar piksela wyliczamy z aktualnego formatu bufora.
        var bytesPerPixel = frame.Format.BitsPerPixel / 8;
        var formatName = frame.Format.ToString();
        var buffer = ReadBuffer(frame.Address, rowBytes * height);
        var output = new byte[buffer.Length];

        // Czytamy każdy piksel, przetwarzamy go i zapisujemy do nowego bufora.
        for (var y = 0; y < height; y++)
        {
            var rowStart = y * rowBytes;

            for (var x = 0; x < width; x++)
            {
                var offset = rowStart + x * bytesPerPixel;
                var (r, g, b, a) = ReadPixel(buffer, offset, formatName);
                var (newR, newG, newB, newA) = transform(r, g, b, a);
                WritePixel(output, offset, newR, newG, newB, newA);
            }
        }

        return CreateBitmap(source.PixelSize, output);
    }

    // Kopiuje piksele z pamięci natywnej do zwykłej tablicy bajtów, żeby można było operować na nich w C#.
    private static byte[] ReadBuffer(IntPtr address, int length)
    {
        var buffer = new byte[length];
        Marshal.Copy(address, buffer, 0, length);
        return buffer;
    }

    // Tworzy nowy obraz wynikowy w docelowym formacie RGBA8888
    private static WriteableBitmap CreateBitmap(PixelSize size, byte[] buffer)
    {
        var bitmap = new WriteableBitmap(size, new Vector(96, 96), PixelFormats.Rgba8888, AlphaFormat.Premul);

        using var frame = bitmap.Lock();
        Marshal.Copy(buffer, 0, frame.Address, buffer.Length);

        return bitmap;
    }

    // Odczytuje jeden piksel z bufora, uwzględniając różne kolejności kanałów zależnie od formatu obrazu.
    private static (byte r, byte g, byte b, byte a) ReadPixel(byte[] buffer, int offset, string formatName)
    {
        return formatName switch
        {
            "Bgra8888" => (buffer[offset + 2], buffer[offset + 1], buffer[offset + 0], buffer[offset + 3]),
            "Rgb32" => (buffer[offset + 0], buffer[offset + 1], buffer[offset + 2], 255),
            "Bgr32" => (buffer[offset + 2], buffer[offset + 1], buffer[offset + 0], 255),
            _ => (buffer[offset + 0], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3]),
        };
    }

    // Zapisuje piksel do bufora wynikowego w stałym układzie RGBA8888.
    private static void WritePixel(byte[] buffer, int offset, byte r, byte g, byte b, byte a)
    {
        buffer[offset + 0] = r;
        buffer[offset + 1] = g;
        buffer[offset + 2] = b;
        buffer[offset + 3] = a;
    }
}