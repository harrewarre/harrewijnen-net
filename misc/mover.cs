// A little hackery to move from the old to the new format.

using System;
using System.IO;
using System.Text.Json;

namespace New_folder
{
    class Metadata
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string[] Tags { get; set; }
        public DateTime Created { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var serializerSettings = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var contentRoot = "C:\\Users\\sharr\\Workspace\\blog\\src\\wwwroot\\content";
            var baseDir = new DirectoryInfo(contentRoot);

            var indexFile = Path.Join(contentRoot, "index.json");
            var postsFolder = Path.Join(contentRoot, "posts");
            var imagesFolder = Path.Join(contentRoot, "images");

            var targetRoot = "C:\\Users\\sharr\\Desktop\\New Folder\\parsed";

            if (Directory.Exists(targetRoot))
            {
                Directory.Delete(targetRoot, true);
            }

            Console.WriteLine($"Loading index from {indexFile}");
            var indexJson = File.ReadAllBytes(indexFile);
            var indexMetadata = JsonSerializer.Deserialize<Metadata[]>(indexJson, serializerSettings);

            Console.WriteLine($"Found {indexMetadata.Length} items");

            if (!Directory.Exists(targetRoot))
            {
                Directory.CreateDirectory(targetRoot);
            }

            foreach (var ix in indexMetadata)
            {
                var targetPost = Path.Join(postsFolder, $"{ix.Slug.ToLower()}.md");

                if (!File.Exists(targetPost))
                {
                    Console.WriteLine($"SKIPPED POST {ix.Slug} - Content not found!");
                    continue;
                }

                var postRoot = Path.Join(targetRoot, ix.Slug.ToLower());
                Directory.CreateDirectory(postRoot);
                Console.WriteLine($"Created folder for {ix.Slug}");

                var postContent = File.ReadAllText(targetPost);
                Console.WriteLine($"Loaded content for {ix.Slug}");

                postContent = postContent.Insert(0, $"# {ix.Title}{Environment.NewLine}{Environment.NewLine}");
                Console.WriteLine($"Added title text: {ix.Title}");

                var oldImagePath = $"Images/{ix.Created.ToString("yyyy")}/{ix.Created.ToString("MM")}";
                postContent = postContent.Replace(oldImagePath, ix.Slug.ToLower(), StringComparison.CurrentCultureIgnoreCase);
                Console.WriteLine($"Fixed image paths for {ix.Slug}");

                File.WriteAllText(Path.Join(postRoot, "post.md"), postContent);
                File.WriteAllText(Path.Join(postRoot, "meta.json"), JsonSerializer.Serialize(ix, serializerSettings));

                Console.WriteLine($"Content resolved and fixed for {ix.Slug}");

                var oldImagesLocation = Path.Join(imagesFolder, ix.Created.ToString("yyyy"), ix.Created.ToString("MM"));
                var targetImagesLocation = Path.Join(targetRoot, ix.Slug.ToLower());

                CopyAll(new DirectoryInfo(oldImagesLocation), new DirectoryInfo(targetImagesLocation), postContent);
            }
        }

        static void CopyAll(DirectoryInfo source, DirectoryInfo target, string postContent)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                if (!postContent.Contains(fi.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir, postContent);
            }
        }
    }
}
