using Moq;
using NUnit.Framework;
using System.Linq;
using WorkoutTimer.Shared.Commands;
using WorkoutTimer.Shared.Interfaces;

namespace WorkoutTimer.Tests
{
    public class CommandFactoryTests
    {
        [Test]
        public void CommandFactory_WhenGivenEmptyScript_ShouldReturnNoCommands()
        {
            var gui = new Mock<IGui>();
            var script = new Mock<IFile>();
            var fileRepo = new Mock<IFileRepository>();

            fileRepo
                .Setup(t => t.GetFile(It.IsAny<string>()))
                .Returns(script.Object);

            var sut = new CommandFactory(gui.Object, fileRepo.Object);

            var commands = sut.GetCommands("scriptPath");

            Assert.IsEmpty(commands);
        }

        [Test]
        public void CommandFactory_WhenGivenOnlyComments_ShouldReturnNoCommands()
        {
            var gui = new Mock<IGui>();
            var script = new Mock<IFile>();
            var fileRepo = new Mock<IFileRepository>();

            script
                .Setup(t => t.ReadLines())
                .Returns(new[] { "# This is a comment", "  #this is another comment." }); 

            fileRepo
                .Setup(t => t.GetFile(It.IsAny<string>()))
                .Returns(script.Object);

            var sut = new CommandFactory(gui.Object, fileRepo.Object);

            var commands = sut.GetCommands("scriptPath");

            Assert.IsEmpty(commands);
        }

        [Test]
        public void CommandFactory_WhenGivenSet_ShouldRepeatSet()
        {
            var gui = new Mock<IGui>();
            var script = new Mock<IFile>();
            var fileRepo = new Mock<IFileRepository>();

            script
                .Setup(t => t.ReadLines())
                .Returns(new[] { 
                    "3 sets of {", 
                    "break",
                    "}" 
                });

            fileRepo
                .Setup(t => t.GetFile(It.IsAny<string>()))
                .Returns(script.Object);

            var sut = new CommandFactory(gui.Object, fileRepo.Object);

            var commands = sut.GetCommands("scriptPath").ToList();

            Assert.IsTrue(commands.Count == 3);
        }
    }
}