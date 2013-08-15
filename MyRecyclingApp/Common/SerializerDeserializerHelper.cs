using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SearchEngine.Common;

namespace MyRecyclingApp.Common
{
    class SerializerDeserializerHelper
    {

        public static ObservableCollection<NewsByTag> Deserialize(string fileName)
        {
            ObservableCollection<NewsByTag> listData = new ObservableCollection<NewsByTag>(); ;

            if (!DesignerProperties.IsInDesignTool)
            {
                try
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isf.FileExists(fileName))
                        {
                            using (var stream = isf.OpenFile(fileName, System.IO.FileMode.Open))
                            {
                                DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<NewsByTag>));
                                var data = serializer.ReadObject(stream) as ObservableCollection<NewsByTag>;

                                if (data != null && data.Count > 0)
                                {
                                    listData = data;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return listData;
        }

        //public static ObservableCollection<T> Deserialize<T>(string fileName)
        //{
        //    ObservableCollection<T> listData = new ObservableCollection<T>(); ;

        //    if (!DesignerProperties.IsInDesignTool)
        //    {
        //        try
        //        {
        //            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
        //            {
        //                if (isf.FileExists(fileName))
        //                {
        //                    using (var stream = isf.OpenFile(fileName, System.IO.FileMode.Open))
        //                    {
        //                        DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<T>));
        //                        var data = serializer.ReadObject(stream) as ObservableCollection<T>;

        //                        if (data != null && data.Count > 0)
        //                        {
        //                            listData = data;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception(ex.Message);
        //        }
        //    }

        //    //return (T) Convert.ChangeType(listData, typeof(ObservableCollection<T>));
        //    return listData;
        //}


        public static bool SerializerNewsByTag(ObservableCollection<NewsByTag> data, string sFileName)
        {
            bool bResult = false;

            try 
	        {	
                //ESpecificamos el tipo de objeto a serializar
                DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<NewsByTag>));
                using(var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists(sFileName))
                        isf.DeleteFile(sFileName);

                    using(var stream = isf.CreateFile(sFileName))
                    {
                        serializer.WriteObject(stream, data);
                        stream.Close();
                    }
                }
	        }
	        catch (Exception ex)
	        {
		
		        throw new Exception(ex.Message);
	        }

            return bResult;
        }

        public static bool SerializerFavorites(ObservableCollection<NewsByTag> data, string sFileName)
        {
            bool bResult = false;

            try
            {
                //ESpecificamos el tipo de objeto a serializar
                DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<NewsByTag>));
                using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isf.FileExists(sFileName))
                        isf.DeleteFile(sFileName);

                    using (var stream = isf.CreateFile(sFileName))
                    {
                        serializer.WriteObject(stream, data);
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return bResult;
        }
    }
}
