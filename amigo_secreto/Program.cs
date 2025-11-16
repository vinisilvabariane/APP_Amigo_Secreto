using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        // Pasta PAGES na raiz do projeto
        string pastaPages = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "pages");
        pastaPages = Path.GetFullPath(pastaPages); // normaliza

        Console.WriteLine($"Pasta de saída: {pastaPages}");

        // Criar pasta se não existir
        Directory.CreateDirectory(pastaPages);

        // 🔥 APAGAR TUDO dentro de pages/
        Console.WriteLine("\nLimpando pasta pages/ ...");

        foreach (string file in Directory.GetFiles(pastaPages))
            File.Delete(file);

        foreach (string dir in Directory.GetDirectories(pastaPages))
            Directory.Delete(dir, true);

        Console.WriteLine("Pasta pages/ limpa!\n");

        // Sorteio (derangement)
        List<string> sorteados = GerarDerangement(nomes);

        Console.WriteLine("Gerando links individuais...\n");

        // Criar arquivos HTML na pasta pages
        for (int i = 0; i < nomes.Count; i++) {
            string nome = nomes[i];
            string quemTirou = sorteados[i];

            // Nome do arquivo = Nome da pessoa.html
            string arquivo = Path.Combine(pastaPages, $"{nome}.html");

            string html = GerarHtml(nome, quemTirou);

            File.WriteAllText(arquivo, html, Encoding.UTF8);

            Console.WriteLine($"{nome}: {arquivo}");
        }

        Console.WriteLine("\nTudo pronto!");
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

    // HTML da página
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
