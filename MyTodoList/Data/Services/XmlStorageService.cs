using System.Xml.Linq;
using Path = System.IO.Path;

namespace MyTodoList.Data.Services;

public class XmlStorageService
{
    private readonly string _xmlFilesDirectory;

    public XmlStorageService(string xmlFilesDirectory)
    {
        _xmlFilesDirectory = xmlFilesDirectory;
        EnsureFilesExist();
    }

    private void EnsureFilesExist()
    {
        var jobsFilePath = Path.Combine(_xmlFilesDirectory, "Jobs.xml");
        if (!File.Exists(jobsFilePath))
        {
            var defaultJobsXml = new XDocument(new XElement("Jobs"));
            defaultJobsXml.Save(jobsFilePath);
        }

        var categoriesFilePath = Path.Combine(_xmlFilesDirectory, "Categories.xml");
        if (!File.Exists(categoriesFilePath))
        {
            var defaultCategoriesXml = new XDocument(
                new XElement("Categories",
                    new XElement("Category", new XAttribute("id", 1), "Без категорії"),
                    new XElement("Category", new XAttribute("id", 2), "Дім"),
                    new XElement("Category", new XAttribute("id", 3), "Робота"),
                    new XElement("Category", new XAttribute("id", 4), "Навчання")
                )
            );

            defaultCategoriesXml.Save(categoriesFilePath);
        }
    }

    public async Task<XDocument> LoadJobsAsync()
    {
        var filePath = Path.Combine(_xmlFilesDirectory, "Jobs.xml");
        var xmlContent = await File.ReadAllTextAsync(filePath);
        return XDocument.Parse(xmlContent);
    }

    public async Task<XDocument> LoadCategoriesAsync()
    {
        var filePath = Path.Combine(_xmlFilesDirectory, "Categories.xml");
        var xmlContent = await File.ReadAllTextAsync(filePath);
        return XDocument.Parse(xmlContent);
    }

    public async Task SaveJobsAsync(XDocument? document)
    {
        var filePath = Path.Combine(_xmlFilesDirectory, "Jobs.xml");
        if (document != null)
        {
            await File.WriteAllTextAsync(filePath, document.ToString());
        }
    }

    public async Task SaveCategoriesAsync(XDocument? document)
    {
        var filePath = Path.Combine(_xmlFilesDirectory, "Categories.xml");
        if (document != null)
        {
            await File.WriteAllTextAsync(filePath, document.ToString());
        }
    }

}