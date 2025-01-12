using ColorProblem;

var kyiv = new Region("Kyiv"); // Special status city
var chernihiv = new Region("Chernihiv");
var zhytomyr = new Region("Zhytomyr");
var vinnytsia = new Region("Vinnytsia");
var cherkasy = new Region("Cherkasy");
var khmelnytskyi = new Region("Khmelnytskyi");
var chernivtsi = new Region("Chernivtsi");
var rivne = new Region("Rivne");
var volyn = new Region("Volyn");
var lviv = new Region("Lviv");
var ternopil = new Region("Ternopil");
var ivanoFrankivsk = new Region("Ivano-Frankivsk");
var zakarpattia = new Region("Zakarpattia");
var odesa = new Region("Odesa");
var mykolaiv = new Region("Mykolaiv");
var kropyvnytskyi = new Region("Kropyvnytskyi"); // Formerly Kirovohrad
var poltava = new Region("Poltava");
var sumy = new Region("Sumy");
var kharkiv = new Region("Kharkiv");
var luhansk = new Region("Luhansk");
var donetsk = new Region("Donetsk");
var zaporizhzhia = new Region("Zaporizhzhia");
var kherson = new Region("Kherson");
var crimea = new Region("Crimea"); // Autonomous Republic of Crimea
var dnipropetrovsk = new Region("Dnipro Region"); // Dnipropetrovsk renamed


kyiv.AddNeighbours([zhytomyr, chernihiv, cherkasy, poltava, vinnytsia]);
chernihiv.AddNeighbours([kyiv, sumy, poltava]);
zhytomyr.AddNeighbours([kyiv, vinnytsia, khmelnytskyi, rivne]);
vinnytsia.AddNeighbours([kyiv, zhytomyr, khmelnytskyi, chernivtsi, odesa, kropyvnytskyi]);
cherkasy.AddNeighbours([kyiv, vinnytsia, kropyvnytskyi, poltava]);
khmelnytskyi.AddNeighbours([zhytomyr, vinnytsia, rivne, chernivtsi, ternopil]);
chernivtsi.AddNeighbours([khmelnytskyi, ternopil, ivanoFrankivsk, vinnytsia]);
rivne.AddNeighbours([zhytomyr, khmelnytskyi, volyn, lviv, ternopil]);
volyn.AddNeighbours([rivne, lviv]);
lviv.AddNeighbours([volyn, rivne, ternopil, zakarpattia, ivanoFrankivsk]);
ternopil.AddNeighbours([rivne, khmelnytskyi, chernivtsi, ivanoFrankivsk, lviv]);
ivanoFrankivsk.AddNeighbours([chernivtsi, ternopil, lviv, zakarpattia]);
zakarpattia.AddNeighbours([lviv, ivanoFrankivsk]);
odesa.AddNeighbours([vinnytsia, mykolaiv, kherson, kropyvnytskyi]);
mykolaiv.AddNeighbours([odesa, kherson, kropyvnytskyi, dnipropetrovsk]);
kropyvnytskyi.AddNeighbours([cherkasy, vinnytsia, mykolaiv, odesa, poltava, dnipropetrovsk]);
poltava.AddNeighbours([kyiv, cherkasy, sumy, kharkiv, dnipropetrovsk, kropyvnytskyi]);
sumy.AddNeighbours([chernihiv, poltava, kharkiv]);
kharkiv.AddNeighbours([sumy, poltava, dnipropetrovsk, donetsk, luhansk]);
luhansk.AddNeighbours([kharkiv, donetsk]);
donetsk.AddNeighbours([luhansk, kharkiv, zaporizhzhia, dnipropetrovsk]);
zaporizhzhia.AddNeighbours([donetsk, dnipropetrovsk, kherson]);
kherson.AddNeighbours([odesa, mykolaiv, dnipropetrovsk, zaporizhzhia, crimea]);
crimea.AddNeighbours([kherson]);
dnipropetrovsk.AddNeighbours([kharkiv, donetsk, zaporizhzhia, kherson, mykolaiv, kropyvnytskyi, poltava]);


var regionsList = new List<Region>
{
    kyiv, chernihiv, zhytomyr, vinnytsia, cherkasy, khmelnytskyi, chernivtsi,
    rivne, volyn, lviv, ternopil, ivanoFrankivsk, zakarpattia, odesa, mykolaiv,
    kropyvnytskyi, poltava, sumy, kharkiv, luhansk, donetsk, zaporizhzhia, kherson, crimea,
    dnipropetrovsk
};


// bool isColored = BackTrackingColoring.ColorMap(regionsList);

 bool isColored = BeamSearchColoring.ColorMapWithBeamSearch(regionsList, 21, 2);

if (isColored)
{
    Console.WriteLine("Regions successfully colored:");
    
    foreach (var region in regionsList)
    {
        Console.WriteLine($"{region.Name}: {region.Color}");
    }
    
    Console.WriteLine($"Iteration: {BeamSearchColoring.iterations}, Total States Generated: {BeamSearchColoring.totalStatesGenerated}, States in Memory: {BeamSearchColoring.statesInMemory}");
    Console.WriteLine($"Total iterations: {BackTrackingColoring.GetIterationCount()}, Total nodes checked: {BackTrackingColoring.GetTotalNodesChecked()}, Total nodes in memory: {BackTrackingColoring.GetTotalNodesInMemory()}, Dead ends: {BackTrackingColoring.GetDeadEnds()}");

}
