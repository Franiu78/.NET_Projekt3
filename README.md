# projekt3 - Platformy Programistyczne (.NET/Java)

To repozytorium zawiera program do laboratorium 3, który mierzy i porównuje czas mnożenia macierzy w dwóch podejściach: `Parallel` i `Thread`.
Program generuje losowe macierze, wykonuje obliczenia dla różnych liczby wątków, a potem wypisuje tabelę z czasami i przyspieszeniem.
W folderze `Task3` znajduje się osobna aplikacja Avalonia do przetwarzania obrazów: wczytuje plik, uruchamia cztery filtry równolegle i pokazuje miniatury wyników.

## Co jest gdzie

- `Task1/` - zadanie 1 i 2, czyli porównanie mnożenia macierzy dla różnych liczby wątków.
- `Task1/Program.cs` - punkt startowy aplikacji, parsowanie argumentów, uruchomienie benchmarku i wypisanie tabel wyników.
- `Task1/Matrix.cs` - klasa macierzy: tworzenie losowych danych, wypisywanie i porównywanie wyników.
- `Task1/MatrixMultiplier.cs` - algorytmy mnożenia macierzy dla `Parallel` i `Thread`.
- `Task1/Benchmark.cs` - wielokrotne pomiary czasu i liczenie średniej.
- `Task3/` - osobna aplikacja Avalonia do zadania 3.
- `Task3/MainWindow.axaml` - układ okna z podziałem na oryginał i wyniki filtrów.
- `Task3/MainWindow.axaml.cs` - logika wczytywania obrazu i równoległego uruchamiania filtrów.
- `Task3/ImageFilters.cs` - właściwa logika filtrów obrazu.


## Wymagania

- .NET SDK 8.0+

## Jak odpalić

### 1) Uruchomienie domyślne

Z katalogu głównego repozytorium:

```bash
dotnet run --project Task1
```

### 2) Uruchomienie z parametrami

```bash
dotnet run --project Task1 -- <rozmiar> <liczba_powtorzen> <lista_watkow>
```

Przykład:

```bash
dotnet run --project Task1 -- 200 5 1,2,4,8,16
```

Znaczenie parametrów:

- `<rozmiar>` - rozmiar macierzy kwadratowej, np. `200` oznacza `200x200`.
- `<liczba_powtorzen>` - ile razy wykonać pomiar dla każdej konfiguracji.
- `<lista_watkow>` - lista liczb oddzielona przecinkami, np. `1,2,4,8`.

### 3) Podgląd macierzy (debug)

Dla małych rozmiarów można wypisać macierze wejściowe i wynikową:

```bash
dotnet run --project Task1 -- 4 2 1,2,4 --show
```

Uwaga: opcji `--show` nie używaj przy realnych pomiarach, bo wypisywanie psuje czasy.

## Jak uruchomić zadanie 3

Z katalogu głównego repozytorium:

```bash
dotnet run --project Task3
```

Po uruchomieniu:

- kliknij `Wczytaj obraz`,
- wybierz plik graficzny,
- kliknij `Uruchom filtry`,
- po lewej zobaczysz oryginał, a po prawej cztery wyniki filtrów.



