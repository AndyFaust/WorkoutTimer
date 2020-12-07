using Moq;
using NUnit.Framework;
using System.Linq;
using WorkoutTimer.Shared.Commands;
using WorkoutTimer.Shared.Interfaces;
using WorkoutTimer.Shared.Sounds;

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
            var soundFactory = new Mock<ISoundFactory>();

            fileRepo
                .Setup(t => t.GetFile(It.IsAny<string>()))
                .Returns(script.Object);

            var sut = new CommandFactory(gui.Object, fileRepo.Object, soundFactory.Object);

            var commands = sut.GetCommands("scriptPath");

            Assert.IsEmpty(commands);
        }

        [Test]
        public void CommandFactory_WhenGivenOnlyComments_ShouldReturnNoCommands()
        {
            var gui = new Mock<IGui>();
            var script = new Mock<IFile>();
            var fileRepo = new Mock<IFileRepository>();
            var soundFactory = new Mock<ISoundFactory>();

            script
                .Setup(t => t.ReadLines())
                .Returns(new[] { "# This is a comment", "  #this is another comment." }); 

            fileRepo
                .Setup(t => t.GetFile(It.IsAny<string>()))
                .Returns(script.Object);

            var sut = new CommandFactory(gui.Object, fileRepo.Object, soundFactory.Object);

            var commands = sut.GetCommands("scriptPath");

            Assert.IsEmpty(commands);
        }

        [Test]
        public void CommandFactory_WhenGivenSet_ShouldRepeatSet()
        {
            var gui = new Mock<IGui>();
            var script = new Mock<IFile>();
            var fileRepo = new Mock<IFileRepository>();
            var soundFactory = new Mock<ISoundFactory>();

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

            var sut = new CommandFactory(gui.Object, fileRepo.Object, soundFactory.Object);

            var commands = sut.GetCommands("scriptPath").ToList();

            Assert.IsTrue(commands.Count == 3);
        }

        [Test]
        public void CommandFactory_WhenGivenScriptWithMp3_ShouldCreateCommandWithMp3()
        {
            var gui = new Mock<IGui>();
            var script = new Mock<IFile>();
            var fileRepo = new Mock<IFileRepository>();
            var mp3 = new Mock<IFile>();
            var soundFactory = new Mock<ISoundFactory>();

            script
                .Setup(t => t.ReadLines())
                .Returns(new[] {
                    "Pushups, 30, test.mp3"
                });

            script
                .Setup(t => t.Directory)
                .Returns(@"this\is\the\directory");

            soundFactory
                .Setup(n => n.GetSoundFromFile(It.IsAny<IFile>()))
                .Returns(new NullSound());

            fileRepo
                .Setup(t => t.GetFile("script.txt"))
                .Returns(script.Object);

            fileRepo
                .Setup(t => t.GetFile(@"this\is\the\directory\test.mp3"))
                .Returns(mp3.Object);

            var sut = new CommandFactory(gui.Object, fileRepo.Object, soundFactory.Object);

            var commands = sut.GetCommands("script.txt").ToList();

            fileRepo.Verify(n => n.GetFile(@"this\is\the\directory\test.mp3"), Times.Once);
            soundFactory.Verify(n => n.GetSoundFromFile(It.IsAny<IFile>()), Times.Once);
        }
    }
}