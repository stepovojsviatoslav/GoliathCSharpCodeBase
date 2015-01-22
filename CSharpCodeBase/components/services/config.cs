 namespace MainGame{

 public class config 
 {
     private string _table;
     private string _section;

     public void init(string table, string section)
     {
         _table = table;
         _section = section;
     }

     public object Get(string path)
     {
         return GameController.database.Get(_table, string.Format("{0}/{1}", _section, path));
     }
 }