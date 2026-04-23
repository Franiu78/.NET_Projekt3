using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Platform;
using System.Threading.Tasks;

namespace Task3;

public partial class MainWindow : Window
{
    // Przechowuje obraz wejściowy i wyniki czterech filtrów
    private WriteableBitmap? currentImage;
    private WriteableBitmap? grayscaleImage;
    private WriteableBitmap? negativeImage;
    private WriteableBitmap? thresholdImage;
    private WriteableBitmap? mirrorImage;

    public MainWindow()
    {
        InitializeComponent();
    }

    // Otwiera okno wyboru pliku i wczytuje obraz do podglądu po lewej stronie.
    private async void LoadImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Użytkownik wybiera tylko pliki obrazów, żeby nie trzeba było obsługiwać błędnych formatów.
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Wybierz obraz",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Obrazy PNG/JPG")
                {
                    Patterns = ["*.png", "*.jpg", "*.jpeg", "*.bmp"]
                }
            ]
        });

        if (files.Count == 0)
        {
            StatusText.Text = "Nie wybrano pliku.";
            return;
        }

        // WriteableBitmap.Decode daje nam obiekt, który później można czytać i przetwarzać piksel po pikselu.
        await using var stream = await files[0].OpenReadAsync();
        var sourceBitmap = WriteableBitmap.Decode(stream);

        SetCurrentImage(sourceBitmap);
        ClearFilterPreviews();

        StatusText.Text = $"Wczytano: {files[0].Name}";
    }

    // Uruchamia cztery filtry równolegle i pokazuje ich wyniki w panelu po prawej.
    private async void RunFilters_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (currentImage is null)
        {
            StatusText.Text = "Najpierw wczytaj obraz.";
            return;
        }

        StatusText.Text = "Uruchamiam filtry...";

        // Każdy filtr startuje na osobnym zadaniu z puli wątków, więc obliczenia lecą równolegle.
        var sourceImage = currentImage;
        var grayscaleTask = Task.Run(() => ImageFilters.Grayscale(sourceImage));
        var negativeTask = Task.Run(() => ImageFilters.Negative(sourceImage));
        var thresholdTask = Task.Run(() => ImageFilters.Threshold(sourceImage));
        var mirrorTask = Task.Run(() => ImageFilters.MirrorHorizontal(sourceImage));

        // Czekamy na wszystkie zadania
        await Task.WhenAll(grayscaleTask, negativeTask, thresholdTask, mirrorTask);

        SetFilterPreview(ref grayscaleImage, GrayscaleImage, grayscaleTask.Result);
        SetFilterPreview(ref negativeImage, NegativeImage, negativeTask.Result);
        SetFilterPreview(ref thresholdImage, ThresholdImage, thresholdTask.Result);
        SetFilterPreview(ref mirrorImage, MirrorImage, mirrorTask.Result);

        StatusText.Text = "Filtry zostały uruchomione.";
    }

    // Ustawia obraz wejściowy w głównym podglądzie i zwalnia poprzedni, żeby nie zostawiać zbędnych bitmap w pamięci.
    private void SetCurrentImage(WriteableBitmap image)
    {
        currentImage?.Dispose();
        currentImage = image;
        PreviewImage.Source = currentImage;
    }

    // Podmienia wynik konkretnego filtra i ustawia go w odpowiedniej kontrolce.
    private static void SetFilterPreview(ref WriteableBitmap? field, Image target, WriteableBitmap? image)
    {
        field?.Dispose();
        field = image;
        target.Source = field;
    }

    // Czyści wszystkie miniatury wyników po wczytaniu nowego obrazu, żeby nie pokazywać starego zestawu filtrów.
    private void ClearFilterPreviews()
    {
        SetFilterPreview(ref grayscaleImage, GrayscaleImage, null);
        SetFilterPreview(ref negativeImage, NegativeImage, null);
        SetFilterPreview(ref thresholdImage, ThresholdImage, null);
        SetFilterPreview(ref mirrorImage, MirrorImage, null);
    }
}