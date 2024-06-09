using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyTodoList.Data.Models;
using MyTodoList.Data.ViewModels;
using MyTodoList.Repositories;

namespace MyTodoList.Controllers;

public class TodoController(
    RepositorySwitcher<Job, int> jobRepository,
    RepositorySwitcher<Category, int> categoryRepository)
    : Controller
{
    public async Task<IActionResult> Todo()
    {
        var model = new TodoViewModel
        {
            CurrentRepositoryType = jobRepository.GetRepositoryType(),
            Jobs = (await jobRepository.GetAllAsync())
                .OrderByDescending(job => job.IsDone)
                .ThenByDescending(job => job.Id)
        };

        ViewBag.Categories = await GetCategoriesSelectList();
        
        return View(model);
    }

    private async Task<SelectList> GetCategoriesSelectList()
    {
        var categories = (await categoryRepository.GetAllAsync()).Where(c => c.Id != 1);
        return new SelectList(categories, "Id", "Name");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Todo(TodoViewModel model)
    {
        var job = model.Job;

        if (string.IsNullOrEmpty(job.Name) || job.CategoryId == 0)
        {
            ViewBag.Categories = await GetCategoriesSelectList();
            model.Jobs = await jobRepository.GetAllAsync();
            return View(model);
        }

        await jobRepository.CreateAsync(job);

        return RedirectToAction("Todo");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeProgress(int id)
    {
        var job = await jobRepository.GetByIdAsync(id);
        if (job.IsDone) return RedirectToAction("Todo");
        job.IsDone = true;
        await jobRepository.UpdateAsync(job);
        return RedirectToAction("Todo");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await jobRepository.DeleteAsync(id);
        return RedirectToAction("Todo");
    }

    [HttpPost]
    [Route("todo/switch")]
    public IActionResult Switch(string repositoryType, string? returnUrl)
    {
        switch (repositoryType.ToLower())
        {
            case "sql":
                jobRepository.SwitchToSql();
                categoryRepository.SwitchToSql();
                break;
            case "xml":
                jobRepository.SwitchToXml();
                categoryRepository.SwitchToXml();
                break;
        }

        return Redirect(returnUrl!);
    }
}