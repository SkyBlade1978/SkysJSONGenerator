using System.Collections.Generic;

namespace SkysJSONGenerator
{
    public class TemplateData
    {
         public string Path { get; set; }
         public string Name { get; set; }
         public string BlockName { get; set; }
         public string MaterialName { get; set; }
         public string TopSuffix { get; set; }
         public string SideSuffix { get; set; }
         public string WallList { get; set; }
         public string LangName { get; set; }
         public string[] Textures { get; set; }
         public string SmoothSuffix { get; set; }
         public string BrickSuffix { get; set; }
         public List<KeyValuePair<string, int>> Ingredients;
         public string Conditions { get; set; }
         public string IngredientDomain { get; set; }
         public string ColourMeta { get; set; }

        private TemplateData Clone()
         {
             return new TemplateData
             {
                 Path = this.Path, Name = this.Name, BlockName = this.BlockName, MaterialName = this.MaterialName,
                 TopSuffix = this.TopSuffix, SideSuffix = this.SideSuffix, WallList = this.WallList,
                 LangName = this.LangName, Textures = this.Textures, SmoothSuffix = this.SmoothSuffix,
                 BrickSuffix = this.BrickSuffix, Ingredients = this.Ingredients, Conditions = this.Conditions,
                 IngredientDomain = this.IngredientDomain, ColourMeta = this.ColourMeta
             };
         }

         public TemplateData WithPath(string path)
         {
             var data = this.Clone();
             data.Path = path;
             return data;
         }

         public TemplateData WithName(string name)
         {
             var data = this.Clone();
             data.Name = name;
             return data;
         }

         public TemplateData WithBlockName(string blockName)
         {
             var data = this.Clone();
             data.BlockName = blockName;
             return data;
        }
    }
}
