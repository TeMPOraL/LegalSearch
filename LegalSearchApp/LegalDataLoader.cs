using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

public static class LegalDataLoader
{
    public static async Task<IEnumerable<LegalDocument>> LoadGithubLicenses()
    {
        // Use GitHub API to fetch popular license texts.
        // No auth needed for public data.

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "LegalSearchApp");
        //client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.+json");
        //client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

        var response = await client.GetAsync("https://api.github.com/licenses");
        if (response.StatusCode != HttpStatusCode.OK)
        {
            // FIXME log properly - but will do for now.
            Console.Error.WriteLine($"Error fetching licenses: {response}");
            return Enumerable.Empty<LegalDocument>();
        }

        var content = await response.Content.ReadAsStringAsync();

        var entries = JsonSerializer.Deserialize<IEnumerable<GithubLicenseEntry>>(content, DeserializerOptions);

        var foo = entries.Select(async (e) =>
        {
            var licenseText = await FetchLicenseText(client, e.Url);
            return new LegalDocument(e.Key, e.Name, licenseText, "License");
        });
        return await Task.WhenAll(foo);

    }

    private static async Task<string> FetchLicenseText(HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            // FIXME log properly - but will do for now.
            Console.Error.WriteLine($"Error fetching license text: {response}");
            return string.Empty;
        }

        var content = await response.Content.ReadAsStringAsync();
        var license = JsonSerializer.Deserialize<GithubLicense>(content, DeserializerOptions);
        return license.Body;
    }

    private static readonly JsonSerializerOptions DeserializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        UnmappedMemberHandling = System.Text.Json.Serialization.JsonUnmappedMemberHandling.Skip
    };
}

record GithubLicenseEntry(string Key, string Name, string Url);
record GithubLicense(string Body);