# projekt3 - Platformy Programistyczne (.NET/Java)

To repozytorium zawiera program do laboratorium 3, który mierzy i porównuje czas mnożenia macierzy w dwóch podejściach: `Parallel` i `Thread`.
Program generuje losowe macierze, wykonuje obliczenia dla różnych liczby wątków, a potem wypisuje tabelę z czasami i przyspieszeniem.
W folderze `Task3` znajduje się osobna aplikacja Avalonia do przetwarzania obrazów: wczytuje plik, uruchamia cztery filtry równolegle i pokazuje miniatury wyników.

## Wyniki porównania

Poniżej są wyniki dla wątków `1,2,4,8,12,24,48` przy `ProcessorCount = 12` i `10` powtórzeniach.

### 10 x 10

#### Parallel

| Wątki | Średni czas [ms] | Przyspieszenie | Zgodność z 1-wątkiem |
| --- | ---: | ---: | --- |
| 1 | 0.02 | 1.00 | tak |
| 2 | 0.16 | 0.15 | tak |
| 4 | 0.05 | 0.53 | tak |
| 8 | 0.03 | 0.70 | tak |
| 12 | 0.03 | 0.85 | tak |
| 24 | 0.03 | 0.81 | tak |
| 48 | 0.02 | 1.58 | tak |

#### Thread

| Wątki | Średni czas [ms] | Przyspieszenie | Zgodność z 1-wątkiem |
| --- | ---: | ---: | --- |
| 1 | 0.15 | 1.00 | tak |
| 2 | 0.17 | 0.87 | tak |
| 4 | 0.29 | 0.51 | tak |
| 8 | 0.62 | 0.23 | tak |
| 12 | 0.81 | 0.18 | tak |
| 24 | 0.80 | 0.18 | tak |
| 48 | 0.77 | 0.19 | tak |

#### Porównanie Parallel vs Thread

| Wątki | Parallel [ms] | Thread [ms] | Szybsze podejście |
| --- | ---: | ---: | --- |
| 1 | 0.02 | 0.15 | Parallel |
| 2 | 0.16 | 0.17 | Parallel |
| 4 | 0.05 | 0.29 | Parallel |
| 8 | 0.03 | 0.62 | Parallel |
| 12 | 0.03 | 0.81 | Parallel |
| 24 | 0.03 | 0.80 | Parallel |
| 48 | 0.02 | 0.77 | Parallel |

Obserwacje:
- Dla tak małej macierzy narzut tworzenia i zarządzania wątkami jest większy niż sam koszt obliczeń.
- Wyniki są bardzo małe, więc różnice między wariantami są w praktyce mało istotne.
- `Parallel` wypada tu stabilniej, a `Thread` wyraźnie traci przy większej liczbie wątków.

### 100 x 100

#### Parallel

| Wątki | Średni czas [ms] | Przyspieszenie | Zgodność z 1-wątkiem |
| --- | ---: | ---: | --- |
| 1 | 13.08 | 1.00 | tak |
| 2 | 6.58 | 1.99 | tak |
| 4 | 3.37 | 3.88 | tak |
| 8 | 2.64 | 4.96 | tak |
| 12 | 2.79 | 4.69 | tak |
| 24 | 2.55 | 5.14 | tak |
| 48 | 2.45 | 5.34 | tak |

#### Thread

| Wątki | Średni czas [ms] | Przyspieszenie | Zgodność z 1-wątkiem |
| --- | ---: | ---: | --- |
| 1 | 12.94 | 1.00 | tak |
| 2 | 6.52 | 1.98 | tak |
| 4 | 3.52 | 3.67 | tak |
| 8 | 3.01 | 4.30 | tak |
| 12 | 3.19 | 4.06 | tak |
| 24 | 3.23 | 4.01 | tak |
| 48 | 4.59 | 2.82 | tak |

#### Porównanie Parallel vs Thread

| Wątki | Parallel [ms] | Thread [ms] | Szybsze podejście |
| --- | ---: | ---: | --- |
| 1 | 13.08 | 12.94 | Thread |
| 2 | 6.58 | 6.52 | Thread |
| 4 | 3.37 | 3.52 | Parallel |
| 8 | 2.64 | 3.01 | Parallel |
| 12 | 2.79 | 3.19 | Parallel |
| 24 | 2.55 | 3.23 | Parallel |
| 48 | 2.45 | 4.59 | Parallel |

Obserwacje:
- Dla średniego rozmiaru obu podejść opłaca się używać większej liczby wątków.
- `Parallel` jest tu zwykle szybszy od ręcznie zarządzanych `Thread`, szczególnie przy większym obciążeniu.
- Zbyt duża liczba wątków nie daje już wyraźnego zysku, więc pojawia się efekt nasycenia.

### 500 x 500

#### Parallel

| Wątki | Średni czas [ms] | Przyspieszenie | Zgodność z 1-wątkiem |
| --- | ---: | ---: | --- |
| 1 | 1740.95 | 1.00 | tak |
| 2 | 918.13 | 1.90 | tak |
| 4 | 505.35 | 3.45 | tak |
| 8 | 430.60 | 4.04 | tak |
| 12 | 415.72 | 4.19 | tak |
| 24 | 427.81 | 4.07 | tak |
| 48 | 394.99 | 4.41 | tak |

#### Thread

| Wątki | Średni czas [ms] | Przyspieszenie | Zgodność z 1-wątkiem |
| --- | ---: | ---: | --- |
| 1 | 1858.28 | 1.00 | tak |
| 2 | 1008.20 | 1.84 | tak |
| 4 | 583.26 | 3.19 | tak |
| 8 | 451.81 | 4.11 | tak |
| 12 | 396.86 | 4.68 | tak |
| 24 | 382.66 | 4.86 | tak |
| 48 | 437.72 | 4.25 | tak |

#### Porównanie Parallel vs Thread

| Wątki | Parallel [ms] | Thread [ms] | Szybsze podejście |
| --- | ---: | ---: | --- |
| 1 | 1740.95 | 1858.28 | Parallel |
| 2 | 918.13 | 1008.20 | Parallel |
| 4 | 505.35 | 583.26 | Parallel |
| 8 | 430.60 | 451.81 | Parallel |
| 12 | 415.72 | 396.86 | Thread |
| 24 | 427.81 | 382.66 | Thread |
| 48 | 394.99 | 437.72 | Parallel |

Obserwacje:
- Przy dużej macierzy różnice między strategiami są już wyraźne i sens ma realna równoległość.
- `Parallel` najczęściej prowadzi, ale `Thread` potrafi wygrać w pojedynczych punktach, gdy liczba wątków dobrze trafia w podział pracy.
- Po przekroczeniu pewnego progu dokładanie wątków nie poprawia już wyników liniowo.

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



