using Library.Data;
using System.Xml.Linq;

namespace DocumentArchivingSystem
{
    public class Logic
    {
        private readonly DASDbContext _db;

        public Logic(DASDbContext db)
        {
            _db = db;
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();

            Console.ReadKey();
        }

        private static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Menu()
        {
            using (var db = new DASDbContext())
            {
                db.Database.EnsureCreated();

                var logic = new Logic(db);

                Console.Clear();

                Console.WriteLine("Menu: \n");

                Console.WriteLine("[1] Dodaj dokument");
                Console.WriteLine("[2] Usuń dokument");
                Console.WriteLine("[3] Znajdź dokumenty");
                Console.WriteLine("[4] Modyfikuj dokument");
                Console.WriteLine("[5] Zakończ\n");

                Console.Write("Wybór: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        logic.AddDocument();
                        Menu();
                        break;
                    case "2":
                        logic.RemoveDocument();
                        Menu();
                        break;
                    case "3":
                        logic.SearchDocuments();
                        Menu();
                        break;
                    case "4":
                        logic.ModifyDocument();
                        Menu();
                        break;
                    case "5":
                        Environment.Exit(0);
                        break;
                    default:
                        Error("Wprowadzono błędną wartość");
                        Menu();
                        break; 
                }
            }
        }

        public void AddDocument()
        {
            Console.Clear();

            Guid uuid = Guid.NewGuid();

            try
            {
                Console.Write("Podaj tytuł: ");
                string title = Console.ReadLine();

                Console.Write("Podaj rok: ");
                string year = Console.ReadLine();

                Console.Write("Podaj kategorie: ");
                string category = Console.ReadLine();

                Console.Write("Podaj lokalizację: ");
                string localization = Console.ReadLine();

                Console.Write("Liczba egzemplarzy: ");
                int numOfCopies = int.Parse(Console.ReadLine());

                Document document = new Document
                {
                    UUID = uuid,
                    Title = title,
                    Year = year,
                    Category = category,
                    Location = localization,
                    NumOfCopies = numOfCopies
                };

                _db.Documents.Add(document);
                _db.SaveChanges();
                Success("Dokument został dodany");
            }
            catch (Exception)
            {
                Error("\nWprowadzono nieprawidłowe dane");
            }
        }

        public void RemoveDocument()
        {
            Console.Clear();

            if (_db.Documents.Count() == 0)
            {
                Console.WriteLine("Brak dokumentacji do usunięcia");
            }
            else
            {
                Console.Write("Podaj nazwę: ");
                string userChoice = Console.ReadLine();

                var documentToRemove = _db.Documents.FirstOrDefault(doc => doc.Title == userChoice);

                if (documentToRemove != null)
                {
                    _db.Documents.Remove(documentToRemove);
                    _db.SaveChanges();
                    Success("Dokument został usunięty");
                }
                else
                {
                    Console.WriteLine("Nie znaleziono dokumentu o podanej nazwie");
                }
            }

            Console.ReadKey();
        }

        private void ShowAllDocuments()
        {
            Console.Clear();

            Console.WriteLine($"Ilość dokumentów: {_db.Documents.Count()}\n");

            int counter = 0;
            foreach (Document document in _db.Documents)
            {
                Console.WriteLine($"[{++counter}] | UUID: {document.UUID} |  Tytuł: {document.Title} | Rok: {document.Year} | Kategoria: {document.Category} | Lokalizacja: {document.Location} | Liczba egz.: {document.NumOfCopies}");
            }

            Console.ReadKey();
        }

        private void DisplayFilteredDocuments(Func<Document, bool> filter)
        {
            Console.Clear();

            int counter = 0;
            var filteredDocuments = _db.Documents.Where(filter);

            if (filteredDocuments.Any())
            {
                foreach (var document in filteredDocuments)
                {
                    Console.WriteLine($"{++counter}. {document.Title} | {document.Year} | {document.Category} | {document.Location} | {document.NumOfCopies}");
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono dokumentów spełniających kryteria wyszukiwania.");
            }
            Console.ReadKey();
        }

        public void SearchDocuments()
        {
            Console.Clear();

            if (_db.Documents.Count() == 0)
            {
                Console.WriteLine("Brak dokumentacji do wyszukania");
                Console.ReadKey();
            }
            else
            {
                int counter = 0;
                string userChoice;

                Console.WriteLine("Wyszukaj za pomocą:");
                Console.WriteLine("1. Tytułu");
                Console.WriteLine("2. Roku");
                Console.WriteLine("3. Miejsca przechowywania");
                Console.WriteLine("4. Wyświetl listę wszystkich dokumentów");

                Console.Write("\nWybór: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Podaj nazwę: ");
                        userChoice = Console.ReadLine();
                        DisplayFilteredDocuments(d => d.Title.Contains(userChoice));
                        break;
                    case "2":
                        Console.Write("Podaj rok: ");
                        userChoice = Console.ReadLine();
                        DisplayFilteredDocuments(d => d.Year.Contains(userChoice));
                        break;
                    case "3":
                        Console.Write("Podaj miejsce: ");
                        userChoice = Console.ReadLine();
                        DisplayFilteredDocuments(d => d.Location.Contains(userChoice));
                        break;
                    case "4":
                        ShowAllDocuments();
                        break;
                    default:
                        Error("Wprowadzono błędną wartość");
                        break;
                }
            }
        }

        public void ModifyDocument()
        {
            Console.Clear();

            var documents = _db.Documents.ToList();

            if (_db.Documents.Count() == 0)
            {
                Console.WriteLine("Brak dokumentacji do modyfikacji");
                Console.ReadKey();
            }
            else
            {
                Console.Write("Podaj nazwę dokumentu: ");
                string userChoice = Console.ReadLine();

                int counter = 0;

                var documentToModify = documents.FindAll(d => d.Title == userChoice);


                if (documentToModify.Count == 0)
                {
                    Console.WriteLine("Nie znaleziono dokumentacji o podanej nazwie");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wybierz właściwość");

                    Console.WriteLine("[1] Tytuł");
                    Console.WriteLine("[2] Rok");
                    Console.WriteLine("[3] Kategoria");
                    Console.WriteLine("[4] Lokalizacja");
                    Console.WriteLine("[5] Liczba kopii");
                    Console.WriteLine("[6] Powrót do menu");

                    Console.Write("\nWybór: ");

                    foreach (var document in documentToModify)
                    {
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.Write("Nowa nazwa: ");
                                document.Title = Console.ReadLine();
                                Success("Zmiana zaszła pomyślnie");
                                _db.SaveChanges();
                                Console.ReadKey();
                                return;
                            case "2":
                                Console.Write("Nowy rok: ");
                                document.Year = Console.ReadLine();
                                Success("Zmiana zaszła pomyślnie");
                                _db.SaveChanges();
                                Console.ReadKey();
                                return;
                            case "3":
                                Console.Write("Nowa kategoria: ");
                                document.Category = Console.ReadLine();
                                Success("Zmiana zaszła pomyślnie");
                                _db.SaveChanges();
                                Console.ReadKey();
                                return;
                            case "4":
                                Console.Write("Nowa lokalizacja: ");
                                document.Location = Console.ReadLine();
                                Success("Zmiana zaszła pomyślnie");
                                _db.SaveChanges();
                                Console.ReadKey();
                                return;
                            case "5":
                                Console.Write("Nowa liczba kopii: ");
                                int newNumOfCopies = int.Parse(Console.ReadLine());
                                if (newNumOfCopies <= 0)
                                {
                                    Error("Wprowadzono nieprawidłową wartość");
                                }
                                else
                                {
                                    document.NumOfCopies = newNumOfCopies;
                                    Success("Zmiana zaszła pomyślnie");
                                    _db.SaveChanges();
                                    Console.ReadKey();
                                }
                                return;
                            case "6":
                                return;
                            default:
                                Error("Wprowadzono błędną wartość");
                                ModifyDocument();
                                return;
                        }
                    }
                }
                Console.ReadKey();
            }
        }
    }
}
