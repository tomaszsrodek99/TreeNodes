using Microsoft.AspNetCore.Http;

namespace Struktura_drzewiasta.Extensions
{
    public static class SessionExtensions
    {
        // Metoda rozszerzająca, która ustawia wartość logiczną w sesji.
        public static void SetBool(this ISession session, string key, bool value)
        {
            // Konwertuje wartość logiczną na tekst i zapisuje ją w sesji pod danym kluczem.
            session.SetString(key, value.ToString());
        }

        // Metoda rozszerzająca, która odczytuje wartość logiczną z sesji.
        public static bool GetBool(this ISession session, string key)
        {
            // Pobiera tekst z sesji dla danego klucza.
            var value = session.GetString(key);

            // Sprawdza, czy wartość tekstowa istnieje i czy można ją przekonwertować na wartość logiczną.
            // Jeśli tak, zwraca przekonwertowaną wartość logiczną; w przeciwnym razie zwraca wartość domyślną (false).
            return value != null && bool.Parse(value);
        }
    }
}
