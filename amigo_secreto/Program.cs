using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

class Program {
    static void Main() {
        Console.WriteLine("=== AMIGO SECRETO — GERAR LINKS ===\n");

        // Lista dos participantes
        List<string> nomes = new List<string>()
        {
            "Vinicius",
            "Enzo Gabriel",
            "Vitoria",
            "Teresinha",
            "Josie",
            "Jessica",
            "Rodrigo",
            "Enzo Ferdinando",
            "Rafael",
            "Vero",
            "Rosangela"
        };

        // Sorteio (derangement)
        List<string> sorteados = GerarDerangement(nomes);

        // Criação de HTML links
        string pasta = "links_amigo_secreto";
        Directory.CreateDirectory(pasta);

        Console.WriteLine("Gerando links individuais...\n");

        for (int i = 0; i < nomes.Count; i++) {
            string nome = nomes[i];
            string quemTirou = sorteados[i];

            string slug = GerarSlug(nome);
            string token = GerarToken();

            string arquivo = Path.Combine(pasta, $"{slug}_{token}.html");

            string html = GerarHtml(nome, quemTirou);

            File.WriteAllText(arquivo, html, Encoding.UTF8);

            Console.WriteLine($"{nome}: {arquivo}");
        }

        Console.WriteLine("\nTudo pronto!");
        Console.WriteLine("Agora envie cada HTML para o dono do arquivo.");
    }

    // Gera permutação onde ninguém recebe ele mesmo
    static List<string> GerarDerangement(List<string> nomes) {
        Random rnd = new Random();
        List<string> sorteio;

        do {
            sorteio = nomes.OrderBy(x => rnd.Next()).ToList();
        }
        while (ExisteAutoSorteio(nomes, sorteio));

        return sorteio;
    }

    static bool ExisteAutoSorteio(List<string> originais, List<string> sorteados) {
        for (int i = 0; i < originais.Count; i++)
            if (originais[i] == sorteados[i])
                return true;
        return false;
    }

    // "Vinicius" -> "vinicius"
    static string GerarSlug(string nome) {
        return nome.ToLower()
                   .Replace(" ", "_")
                   .Replace("ç", "c");
    }

    // Gera token aleatório tipo "k39dk2"
    static string GerarToken() {
        using (var rng = RandomNumberGenerator.Create()) {
            byte[] bytes = new byte[4];
            rng.GetBytes(bytes);
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    // Gera o conteúdo da página HTML
    static string GerarHtml(string nome, string tirou) {
        return $@"
<!DOCTYPE html>
<html lang='pt-br'>
<head>
<meta charset='UTF-8'>
<title>Amigo Secreto</title>
<style>
body {{
    font-family: Arial, sans-serif;
    background: #fafafa;
    padding: 40px;
    text-align: center;
}}
.box {{
    margin: auto;
    background: white;
    padding: 25px;
    border-radius: 12px;
    width: 300px;
    box-shadow: 0 0 10px rgba(0,0,0,0.15);
}}
</style>
</head>
<body>
    <div class='box'>
        <h2>Olá, {nome}!</h2>
        <p>O seu amigo secreto é:</p>
        <h1 style='color:#d32f2f'>{tirou}</h1>
        <p>Boa sorte!</p>
    </div>
</body>
</html>";
    }
}
