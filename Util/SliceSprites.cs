 /*
 * <CopyRight> Dev.NikuRamen </CopyRight>
 * <License> Mit licence </License>
 * Slice atlas file to each file
 *
 * Email :  dev.alter0@gmail.com
 * Github : github.com/hackplayerz
 */

using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SliceSprites : MonoBehaviour
{
    #region Variables
    
    [Header("Slice Sprite, Create Menu is in context menu")]    
    [Tooltip("File must be in Resources/(folder name)/(file name)")][SerializeField] private string toSliceFilePath; // To slice file Path, file must be in Resources directory
    [Tooltip("Save file path,it must be exist in Asset folder")] [SerializeField] private string toCreateFolderName; // To Create File Name, It must be exist
    [Tooltip("Create file this name as .png, plot as index")] [SerializeField] private string toCreateFileName; // To Create File

    /* ----------------------------------------------------------------- */

    private readonly StringBuilder _path = new StringBuilder();
    
    #endregion
    

    [ContextMenu("Create File")]
    void SliceSprite()
    {
        // Load atlas sprite dynamic
        var sprites = Resources.LoadAll<Sprite>(toSliceFilePath);
        
        // Check sprite is exist
        if(sprites == null)
        {
            Debug.LogError("No Data exist : " + toSliceFilePath);
            Debug.LogError("Check file name or file exist!");
            return;
        }

        // Convert to binary file and create bitmap file
        for(var i=0;i<sprites.Length;i++)
        {
            var sprite = sprites[i]; // Get each sprite as to slice atlas sprite
            
            // Texture of sliced sprite save as texture
            Texture2D texture2D = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height,TextureFormat.ARGB32,false);
            texture2D.SetPixels( 0, 0, texture2D.width, texture2D.height, sprite.texture.GetPixels((int)sprite.rect.x, (int)sprite.rect.y, (int)sprite.rect.width, (int)sprite.rect.height) ); // Start convert as pixel data
            
            texture2D.Apply();
            
            var imageBuffer =  texture2D.EncodeToPNG(); // Encoding to binary data
            _path.AppendFormat("{0}{1}{2}{3}{4}{5}{6}", Application.dataPath,@"/",toCreateFolderName,@"/", toCreateFileName,i,".png"); // Get this file's asset folder and create file
            File.WriteAllBytes(_path.ToString(), imageBuffer); // Make png file
            _path.Clear();
        }
        // To debug
        _path.AppendFormat("{0}{1}{2}{3}{4}", Application.dataPath, @"/", toCreateFolderName, @"/", toCreateFileName);
        Debug.Log("Crated Complete : " + _path);
        AssetDatabase.Refresh(); // Refresh Editor
        _path.Clear();
    }
}
