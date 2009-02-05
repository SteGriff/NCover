using NUnit.Framework;
using SvnTracker.Lang;
using SvnTracker.Model;

namespace SvnTracker.TestData
{
    [TestFixture]
    public class UpToDateSpec
    {
        const string monitoredDir = @"c:\";

        [Test]
        public void WhenShouldShowUpToDate()
        {
            var model = SetupModel(new Stubversion
               {
                   {monitoredDir, GetCurrentDir()},
                   {@"file:///c:/repo/example/trunk", GetCurrentRepo(1, "gilescope")},
                   {@"file:///c:/repo/example/trunk@1", GetLogEntry(1, "gilescope")},
               });
            model.Assert(c=>!c.HasChanges);
        }

        [Test]
        public void WhenRepositryAheadDueToSomeoneElseShouldNeeedUpdating()
        {
            var model = SetupModel(new Stubversion
               {
                   {monitoredDir, GetCurrentDir()},
                   {@"file:///c:/repo/example/trunk", GetCurrentRepo(6, "Mr Dee")},
                   {@"file:///c:/repo/example/trunk@6", GetLogEntry(6, "Mr Dee")},
               });

            string x = null;
            model.Assert(
//                m=>m.HasChanges,
//                m=>m.OutstandingChanges.Count == 1,
//                m=>m.OutstandingChanges[0].User == "Mr Dee"
//                m=>m.OutstandingChanges[0].Revision == 6,
                m=>m.OutstandingChanges[0].IsRelevant
//                m=>x == ""
                );
        }


        [Test]
        public void WhenRepositryAheadDueToSomeoneElseShouldNeeedUpdatingUnlessNotChangedAnyFilesInSubtree()
        {
            var model = SetupModel(new Stubversion
               {
                   {monitoredDir, GetCurrentDir()},
                   {@"file:///c:/repo/example/trunk", GetCurrentRepo(6, "Mr Dee")},
                   {@"file:///c:/repo/example/trunk@6", GetLogEntry(6, "Mr Dee", new []
                  {
                      "<path action='A'>/OtherSubTree/branches</path>",
                  })},
               });

            model.Assert(
                c=>c.HasChanges, 
                c=>c.OutstandingChanges.Count == 1);

            model.OutstandingChanges[0].Assert(
                c=>c.User == "Mr Dee",
                c=>c.Revision == 6,
                c=>!c.IsRelevant
                );
        }

        [Test]
        public void WhenRepositryAheadDueToMeShouldNotNeedUpdating()
        {
            var model = SetupModel(new Stubversion
               {
                   {monitoredDir, GetCurrentDir()},
                   {@"file:///c:/repo/example/trunk", GetCurrentRepo(6, "gilescope")},
                   {@"file:///c:/repo/example/trunk@6", GetLogEntry(6, "gilescope")},
               });

            model.Assert(
                c => c.HasChanges,
                c => c.OutstandingChanges.Count == 1);

            model.OutstandingChanges[0].Assert(
                c => c.User == "gilescope",
                c => c.Revision == 6,
                c => !c.IsRelevant);
        }

        
        #region Hide
        public DirModel SetupModel(Stubversion stubversion)
        {
            var model = new DirModel
            {
                Subversion = stubversion,
                MonitoredDir = monitoredDir
            };

            model.Update();
            return model;
        }


        private static string GetLogEntry(long revision, string author)
        {
            return GetLogEntry(revision, author, new []
                  {
                      "<path action='A'>/example/branches</path>",
                      "<path action='A'>/example/tags</path>",
                      "<path action='A'>/example</path>",
                      "<path action='A'>/example/trunk</path>}"
                  } 
                );
        }

        private static string GetLogEntry(long revision, string author, string[] files)
        {
            return @"<?xml version='1.0'?>
<log>
<logentry
   revision='" + revision + @"'>
<author>"+ author + @"</author>
<date>2009-01-06T18:15:52.733000Z</date>
<paths>

</paths>
<msg>First import</msg>
</logentry>
</log>
";
        }

        private static string GetCurrentRepo(long revision, string user)
        {
            return @"<?xml version='1.0'?>
<info>
<entry
   kind='dir'
   path='trunk'
   revision='" + revision + @"'>
<url>file:///c:/repo/example/trunk</url>
<repository>
<root>file:///c:/repo</root>
<uuid>85349945-3c16-e849-9383-c98c94814b3e</uuid>
</repository>
<commit
   revision='" + revision + @"'>
<author>" + user + @"</author>
<date>2009-01-19T05:56:38.094000Z</date>
</commit>
</entry>
</info>
";
        }

        private static string GetCurrentDir()
        {
            return @"<?xml version='1.0'?>
<info>
<entry
   kind='dir'
   path='.'
   revision='5'>
<url>file:///c:/repo/example/trunk</url>
<repository>
<root>file:///c:/repo</root>
<uuid>85349945-3c16-e849-9383-c98c94814b3e</uuid>
</repository>
<wc-info>
<schedule>normal</schedule>
<depth>infinity</depth>
</wc-info>
<commit
   revision='2'>
<author>gilescope</author>
<date>2009-01-07T09:38:04.825200Z</date>
</commit>
</entry>
</info>
";
        }
        #endregion
    }
}
