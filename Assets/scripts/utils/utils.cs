using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.scripts.utils
{
    public static class utils
    {
        public static float GetPercent(float value, float percent)
        {
            return (value / 100f) * percent;
        }

        public static float GetValuePercent(float value, float maxValue)
        {
            return (value * 100f) / maxValue;
        }

        public static int GetPennyToss()
        {
            var randomNumber = Random.Range(0, 100);
            return (randomNumber > 50) ? 1 : 0;
        }

        //public static float XPercent(float percent)
        //{
        //    return (b.ScreenWidth / 100f) * percent;
        //}

        //public static float YPercent(float percent)
        //{
        //    return (b.ScreenHeight / 100f) * percent;
        //}

        private static int _fontSize;

        public static void Setup()
        {
            if (Screen.width >= 2560)
            {
                //_fontSize = 
            }
        }

        public static int GetFontSize()
        {
            return _fontSize;
        }

        //public static string GetEnumDescription(Enum value)
        //{
        //    FieldInfo fi = value.GetType().GetField(value.ToString());

        //    DescriptionAttribute[] attributes =
        //        (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        //    if (attributes != null && attributes.Length > 0)
        //        return attributes[0].Description;

        //    return value.ToString();
        //}

        public static string GetProfilePictureName(string username, int id)
        {
            return string.Format("{0}_{1}", username.Replace(" ", "_"), id);
        }

        public static string GetProfilePictureName(string username, long id)
        {
            return string.Format("{0}_{1}", username.Replace(" ", "_"), id);
        }

        public static string SavePic(Texture2D pic, int width, int height, string picName)
        {
            string path = Application.persistentDataPath + string.Format("/{0}.png", picName);
            Debug.Log(path);
            try
            {
                byte[] bytes = pic.EncodeToPNG();

                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return path;
        }

        public static Texture2D ReadPic(string picName)
        {
            string path = Application.persistentDataPath + string.Format("/{0}.png", picName);
            Debug.Log(path);

            try
            {
                var bytes = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(128, 128);
                tex.LoadImage(bytes);
                return tex;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            return null;
        }

        // Take a shot immediately
        static IEnumerator Start()
        {
            yield return UploadPNG();
        }

        static IEnumerator UploadPNG(Texture2D pic = null, int width = 0, int height = 0)
        {
            // We should only read the screen buffer after rendering is complete
            yield return new WaitForEndOfFrame();

            if (pic == null)
            {
                // Create a texture the size of the screen, RGB24 format
                if (width < 1)
                    width = Screen.width;
                if (height < 1)
                    height = Screen.height;
                pic = new Texture2D(width, height, TextureFormat.RGB24, false);
            }

            // Read screen contents into the texture
            pic.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            pic.Apply();

            // Encode texture into PNG
            byte[] bytes = pic.EncodeToPNG();
            Object.Destroy(pic);

            // For testing purposes, also write to a file in the project folder
            File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);


            //// Create a Web Form
            //WWWForm form = new WWWForm();
            //form.AddField("frameCount", Time.frameCount.ToString());
            //form.AddBinaryData("fileUpload", bytes);

            //// Upload to a cgi script
            //WWW w = new WWW("http://localhost/cgi-bin/env.cgi?post", form);
            //yield return w;

            //if (w.error != null)
            //{
            //    Debug.Log(w.error);
            //}
            //else
            //{
            //    Debug.Log("Finished Uploading Screenshot");
            //}
        }

        public static string GetDataValue(string data, string index)
        {
            string value = data.Substring(data.IndexOf(index, StringComparison.Ordinal) + index.Length);
            if (value.Contains("|"))
                value = value.Remove(value.IndexOf('|'));
            return value;
        }
        public static int GetIntDataValue(string data, string index)
        {
            int numb;
            var success = int.TryParse(GetDataValue(data, index), out numb);

            return success ? numb : 0;
        }
        public static bool GetBoolDataValue(string data, string index)
        {
            var value = GetDataValue(data, index);
            if (string.IsNullOrEmpty(value) || value.Equals("0"))
                return false;
            return true;
        }
        public static long GetLongDataValue(string data, string index)
        {
            long numb;
            var success = long.TryParse(GetDataValue(data, index), out numb);

            return success ? numb : 0;
        }

        private static int _originalFontSize = 14;

        public static int GetLetterFontSize(RectTransform Rt)
        {
            if (Rt.sizeDelta.x < 91 && Rt.sizeDelta.x > 80)
            {
                return _originalFontSize * 4;
            }
            return _originalFontSize;
        }
    }
}
