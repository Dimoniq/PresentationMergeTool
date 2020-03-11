using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NetOffice.OfficeApi.Enums;
using NetOffice.PowerPointApi.Enums;
using PresentationMergeTool.Contracts;
using PowerPoint = NetOffice.PowerPointApi;

namespace PresentationMergeTool.FileAccess
{
  [ExcludeFromCodeCoverage]
  public class PowerPointAccess : IPowerPointAccess
  {
    private const string PasteFromClipboardCommand = "PasteSourceFormatting";
    private const int WaitAfterSavingMilliseconds = 30;
    private const int WaitAfterCopyingToClipboardMilliseconds = 20;
    private const int WaitAfterPastingFromClipboardMilliseconds = 20;
    private readonly ITaskManager taskManager;

    public PowerPointAccess(ITaskManager taskManager)
    {
      this.taskManager = taskManager;
    }

    public void SavePresentationsAs(IEnumerable<string> presentations, string exportFullFileName)
    {
      using (var powPointDestination = new PowerPoint.Application())
      {
        using (var powPointSource = new PowerPoint.Application())
        {
          var destPresentation = powPointDestination.Presentations.Add(MsoTriState.msoTrue);

          //The newly created presentation has to have at least one slide for the method to work properly
          destPresentation.Slides.Add(1, PpSlideLayout.ppLayoutBlank);

          foreach (var sourcePresentationPath in presentations)
          {
            var sourcePresentation = powPointSource.Presentations.Open(sourcePresentationPath, true);
            this.CopySlidesTo(sourcePresentation, destPresentation);
            this.SavePresentationAs(exportFullFileName, destPresentation);
            sourcePresentation.Close();
          }

          //We need to delete the first empty slide added at the beginning
          this.DeleteSlideAt(destPresentation, 1);

          this.SavePresentationAs(exportFullFileName, destPresentation);

          destPresentation.Close();

          // close power point and dispose reference
          powPointSource.Quit();
          powPointDestination.Quit();
        }
      }
    }

    private void CopySlidesTo(PowerPoint.Presentation sourcePresentation, PowerPoint.Presentation destPresentation)
    {
      if (sourcePresentation == null || destPresentation == null)
      {
        return;
      }

      for (var i = 1; i <= sourcePresentation.Slides.Count; i++)
      {
        this.CopySlideToClipboard(sourcePresentation, i);

        this.AppendSlideFromClipboard(destPresentation);
      }
    }

    private void DeleteSlideAt(PowerPoint.Presentation presentation, int position)
    {
      if (presentation == null || position < 1 || presentation.Slides.Count < position)
      {
        return;
      }

      presentation.Windows[1].Activate();
      presentation.Slides[position].Delete();
    }

    private void SavePresentationAs(string exportFullFileName, PowerPoint.Presentation presentation)
    {
      presentation.SaveAs(exportFullFileName);
      this.taskManager.Sleep(WaitAfterSavingMilliseconds);
    }

    private void AppendSlideFromClipboard(PowerPoint.Presentation destPresentation)
    {
      destPresentation.Windows[1].Activate();
      destPresentation.Windows[1].View.GotoSlide(destPresentation.Slides.Count);
      destPresentation.Application.CommandBars.ExecuteMso(PasteFromClipboardCommand);
      this.taskManager.Sleep(WaitAfterPastingFromClipboardMilliseconds);
    }

    private void CopySlideToClipboard(PowerPoint.Presentation srcPresentation, int index)
    {
      srcPresentation.Windows[1].Activate();
      srcPresentation.Windows[1].View.GotoSlide(index);
      srcPresentation.Slides[index].Copy();
      this.taskManager.Sleep(WaitAfterCopyingToClipboardMilliseconds);
    }
  }
}