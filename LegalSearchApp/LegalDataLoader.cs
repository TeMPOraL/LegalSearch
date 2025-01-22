public static class LegalDataLoader {
    public static async Task<IEnumerable<LegalDocument>> LoadGithubLicenses() {
        // Use GitHub API to fetch popular license texts.
        // No auth needed for public data.
        return null;
    }
}