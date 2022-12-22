// Get list of files in the specific directory.
// ... Please change the first argument.
string[] files = Directory.GetFiles(@"C:\Users\rkbanda\Desktop\PART 1 - Copy\",
    "*.*",
    SearchOption.AllDirectories); ;



// Display all the files.
foreach (string file in files)
{
    FileInfo lobjFile = new FileInfo(file);

    string newfilename = string.Concat(lobjFile.Name.Substring(0, 5), "00", lobjFile.Name.Substring(5));
    lobjFile.MoveTo(Path.Combine(lobjFile.DirectoryName, newfilename));
}