using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using File = System.IO.File;

namespace Editor
{
    public static class BitcodeDisableWorkFlow  
    {
        [PostProcessBuild]
        private static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.iOS)
            {
                var pbxProjectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
                var content = File.ReadAllText(pbxProjectPath);
                content = content.Replace("ENABLE_BITCODE = YES", "ENABLE_BITCODE = NO");
                File.WriteAllText(pbxProjectPath, content);
            }
        }
    }
}
