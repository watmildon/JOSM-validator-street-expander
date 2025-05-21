using System.Text;
using System.Xml.Linq;

namespace JOSM_validator_street_expander
{
    internal class Program
    {
        static Dictionary<string, string> abbreviations = new Dictionary<string, string>()
        {
            {"Acc","Access"},
            {"Aly", "Alley" },
            {"Ambl", "Amble" },
            {"App","Approach" },
            {"Arc","Arcade"},
            {"Artl","Arterial" },
            {"Arty","Artery" },
            {"Av","Avenue"},
            {"Ave","Avenue"},
            {"Bch","Beach" },
            {"Bg","Burg" },
            {"Bgs","Burgs" },
            {"Blf","Bluff"},
            {"Blk","Block" },
            {"Blv","Boulevard"},
            {"Blvd","Boulevard"},
            {"Bnd","Bend"},
            {"Br","Bridge"},
            {"Brg","Bridge"},
            {"Btm","Bottom" },
            {"Bwlk","Boardwalk" },
            {"Byp","Bypass"},
            {"Bypa","Bypass"},
            {"Byu","Byou" },
            {"Bywy","Byway" },
            {"Bzr","Bazaar" },
            {"Cct","Circuit" },
            {"Ch","Chase" },
            {"Cir","Circle"},
            {"Cirs","Circles"},
            {"Clf","Cliff" },
            {"Clfs","Cliffs" },
            {"Cly","Colony" },
            {"Cmn","Common" },
            {"Cnl","Canal" },
            {"Cnr","Corner" },
            {"Coll","College" },
            {"Cpe","Cape" },
            {"Cr","Creek"},
            {"Crk","Creek"},
            {"Cres","Crescent"},
            {"Crst","Crest"},
            {"Cswy","Causeway"},
            {"Ct","Court"},
            {"Ctr","Center"},
            {"Cts","Courts"},
            {"Ctyd","Courtyard"},
            {"Curv","Curve"},
            {"Cutt","Cutting" },
            {"Cv","Cove"},
            {"Cyn","Canyon" },
            {"Dl","Dale" },
            {"Dr","Drive"},
            {"Dv","Drive"},
            {"Dvwy","Driveway" },
            {"Elb","Elbow" },
            {"Est","Estate" },
            {"Ests","Estates" },
            {"Expy","Expressway"},
            {"Expwy","Expressway"},
            {"Fawy","Fairway" },
            {"Fld","Field"},
            {"Flds","Fields"},
            //{"Fls","Falls"}, collision
            //{"Fls","Flats"}, collision
            {"Fmrd","Farm to Market Road"},
            {"Ftrl","Firetrail"},
            {"Frk","Fork" },
            {"Fry","Ferry" },
            {"Fwy","Freeway"},
            {"Gd","Grade"},
            {"Gdns","Gardens"},
            {"Gr","Grove"},
            {"Gro","Grove"},
            {"Grv","Grove"},
            {"Hbr","Harbor"},
            {"Hl","Hill"},
            {"Hls","Hills"},
            {"Holw","Hollow"},
            {"Hts", "Heights" },
            {"Hw","Highway"},
            {"Hwy","Highway"},
            {"Hvn","Haven" },
            {"Intg","Interchange" },
            //{"Is", "Island"}, collision with the english word "is"
            {"Iss", "Islands"},
            {"Jct","Junction" },
            {"Jn","Junction" },
            {"Jnc","Junction" },
            {"Knl","Knoll" },
            {"Knls","Knolls" },
            {"Ky","Key" },
            {"Kys","Keys" },
            {"Lck","Lock" },
            {"Lcks","Locks" },
            {"Ldg","Lodge" },
            {"Lk","Lake" },
            {"Lks","Lakes" },
            {"Lkt","Lookout" },
            {"Ln","Lane"},
            {"Lndg","Landing"},
            {"Lp","Loop"},
            {"Mal","Mall"},
            {"Mdw","Meadow" },
            {"Mdws","Meadows" },
            {"Mkt","Market" },
            {"Ml","Mill" },
            {"Mt","Mount"},
            {"Mtn","Mountain" },
            {"Mtwy","Motorway"},
            {"Orch","Orchard" },
            {"Ovps","Overpass"},
            {"Piaz","Piazza" },
            {"Pk","Peak" },
            {"Pky","Parkway"},
            {"Pkwy","Parkway"},
            {"Pl","Place"},
            {"Pln","Plain" },
            {"Plns","Plains" },
            {"Plz","Plaza"},
            {"Pnt","Point" },
            {"Prkwy","Parkway"},
            {"Pt","Point" },
            {"Pvt","Private"},
            {"Qdrt","Quadrant"},
            {"Qtrs","Quarters"},
            {"Qy","Quay"},
            {"Qys","Quays"},
            {"Rd","Road"},
            {"Rds","Roads"},
            {"Rdg","Ridge"},
            {"Rdge","Ridge"},
            {"Rdgs","Ridges"},
            {"Rw","Row"},
            {"Rmrd","Ranch to Market Road"},
            {"Rt","Route"},
            {"Rte","Route"},
            {"Rty","Rotary"},
            {"Shl","Shoal"},
            {"Shls","Shoals"},
            {"Shr","Shore"},
            {"Shrs","Shores"},
            {"Skwy","Skyway"},
            {"Smt","Summit"},
            {"Spg","Spring"},
            {"Spgs","Springs"},
            {"Sq","Square"},
            {"Sqs","Squares"},
            {"Srvc","Service"},
            {"St","Street" },
            {"Tce","Terrace"},
            {"Ter","Terrace"},
            {"Tfwy","Trafficway"},
            {"Thfr","Thoroughfare"},
            {"Thwy","Throughway"},
            {"Tl","Trail"},
            {"Tlwy","Tollway"},
            {"Tpke","Turnpike"},
            {"Trce","Trace"},
            {"Tr","Trail"},
            {"Trk","Track"},
            {"Trl","Trail"},
            {"Tunl","Tunnel"},
            {"Unp","Underpass"},
            {"Vl","Villa"},
            {"Vlg","Village"},
            {"Vlgs","Villages"},
            {"Vly","Valley"},
            {"Vw","View"},
            {"Wd","Wood" },
            {"Whrf","Wharf" },
            {"Wkwy","Walkway"},
            {"Wlk","Walk" },
            {"Wy","Way"},
            {"Xing","Crossing"}
        };

        
        static void Main(string[] args)
        {
            var output = new StringBuilder();

            
            // meta section
            output.AppendLine("meta");
            output.AppendLine("{");
            output.AppendLine("    title: \"(US) Abbreviated Streetname Fixup\";");
            output.AppendLine($"    version: \"0,1_{DateTime.Now.ToShortDateString()}\";");
            output.AppendLine("    description: \"Expands common street name abbreviations in the US\";");
            output.AppendLine("    author: \"watmildon\";");
            output.AppendLine("    link: \"https://github.com/watmildon/josm-validator-rules/blob/main/rules/USStreetNameExpander.validator.mapcss\";");
            output.AppendLine("    baselanguage: \"en\";");
            output.AppendLine("    min-josm-version: 14481;");
            output.AppendLine("}");
            output.AppendLine();

            // Write Overpass Query
            string postfixRegex = "";
            string postfixPeriodRegex = "";
            string prefixRegex = "";
            string middleRegex = "";

            foreach (var kvp in abbreviations)
            {
                postfixRegex += $" {kvp.Key}$|";
                postfixPeriodRegex += $" {kvp.Key}\\.$|";
                prefixRegex += $"^{kvp.Key} |";
                middleRegex += $" {kvp.Key} |";
            }

            postfixRegex = postfixRegex.Substring(0, postfixRegex.Length-1); // strip last pipe
            postfixPeriodRegex = postfixPeriodRegex.Substring(0, postfixPeriodRegex.Length-1); // strip last pipe
            prefixRegex = prefixRegex.Substring(0, prefixRegex.Length-1); // strip last pipe
            middleRegex = middleRegex.Substring(0, middleRegex.Length-1); // strip last pipe

            output.AppendLine("/*");
            output.AppendLine("[out:json][timeout:300];");
            output.AppendLine("{{geocodeArea:\"United States of America\"}}->.a;");
            output.AppendLine("(");
            output.AppendLine($"  wr[highway][highway!=platform][highway!=\"bus_stop\"][amenity!=shelter][!bus][name~\"{postfixRegex}\"](area.a);");
            output.AppendLine($"  wr[highway][highway!=platform][highway!=\"bus_stop\"][amenity!=shelter][!bus][name~\"{postfixPeriodRegex}\"](area.a);");
            output.AppendLine($"  wr[highway][highway!=platform][highway!=\"bus_stop\"][amenity!=shelter][!bus][name~\"{prefixRegex}\"](area.a);");
            output.AppendLine($"  wr[highway][highway!=platform][highway!=\"bus_stop\"][amenity!=shelter][!bus][name~\"{middleRegex}\"](area.a);");
            output.AppendLine($"  nwr[\"addr:street\"~\"{postfixRegex}\"](area.a);");
            output.AppendLine($"  nwr[\"addr:street\"~\"{postfixPeriodRegex}\"](area.a);");
            output.AppendLine($"  nwr[\"addr:street\"~\"{prefixRegex}\"](area.a);");
            output.AppendLine($"  nwr[\"addr:street\"~\"{middleRegex}\"](area.a);");
            output.AppendLine(");");
            output.AppendLine("out body;");
            output.AppendLine(">;");
            output.AppendLine("out skel qt;");
            output.AppendLine("*/");
            output.AppendLine();

            output.AppendLine("*[\"highway\"][\"name\"][\"highway\"!=\"bus_stop\"] {");
            output.AppendLine("    set highway_name;");
            output.AppendLine("}");
            output.AppendLine();


            // Postfix generation, no period
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"addr:street\"][\"addr:street\"=~/ {kvp.Key}$/] {{");
                output.AppendLine($"assertNoMatch: \"way \\\"addr:street\\\"=Main {kvp.Value}\";");
                output.AppendLine($"assertMatch: \"way \\\"addr:street\\\"=Main {kvp.Key}\";");
                output.AppendLine($"throwWarning: tr(\"addr:street={{0}} contains postfix {kvp.Key}, should likely be expanded to {kvp.Value}\",\"{{0.value}}\");");
                output.AppendLine($"fixAdd: concat(\"addr:street=\", substring(tag(\"addr:street\"), 0, length(tag(\"addr:street\"))-{kvp.Key.Length}), \"{kvp.Value}\");");
                output.AppendLine($"group: tr(\"addr:street contains postfix {kvp.Key}, should likely be expanded to {kvp.Value}\");");
                output.AppendLine("}");
                output.AppendLine();
            }
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"name\"=~/ {kvp.Key}$/].highway_name {{");
                output.AppendLine($"assertNoMatch: \"way \\\"name\\\"=Main {kvp.Value}\";");
                output.AppendLine($"assertMatch: \"way \\\"name\\\"=Main {kvp.Key}\";");
                output.AppendLine($"throwWarning: tr(\"Highway name contains postfix {kvp.Key}, should likely be expanded to {kvp.Value}\");");
                output.AppendLine($"fixAdd: concat(\"name=\", substring(tag(\"name\"), 0, length(tag(\"name\"))-{kvp.Key.Length}), \"{kvp.Value}\");");
                output.AppendLine("}");
                output.AppendLine();
            }

            // Postfix generation, period
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"addr:street\"][\"addr:street\"=~/ {kvp.Key}\\.$/] {{");
                output.AppendLine($"assertNoMatch: \"way \\\"addr:street\\\"=Main {kvp.Value}\";");
                output.AppendLine($"assertMatch: \"way \\\"addr:street\\\"=Main {kvp.Key}.\";");
                output.AppendLine($"throwWarning: tr(\"addr:street={{0}} contains prefix {kvp.Key}., should likely be expanded to {kvp.Value}\",\"{{0.value}}\");");
                output.AppendLine($"fixAdd: concat(\"addr:street=\", substring(tag(\"addr:street\"), 0, length(tag(\"addr:street\"))-{kvp.Key.Length + 1}), \"{kvp.Value}\");");
                output.AppendLine($"group: tr(\"addr:street contains postfix {kvp.Key}, should likely be expanded to {kvp.Value}\");");
                output.AppendLine("}");
                output.AppendLine();
            }
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"name\"=~/ {kvp.Key}\\.$/].highway_name {{");
                output.AppendLine($"assertNoMatch: \"way \\\"name\\\"=Main {kvp.Value}\";");
                output.AppendLine($"assertMatch: \"way \\\"name\\\"=Main {kvp.Key}.\";");
                output.AppendLine($"throwWarning: tr(\"Highway name contains postfix {kvp.Key}., should likely be expanded to {kvp.Value}\");");
                output.AppendLine($"fixAdd: concat(\"name=\", substring(tag(\"name\"), 0, length(tag(\"name\"))-{kvp.Key.Length + 1}), \"{kvp.Value}\");");
                output.AppendLine("}");
                output.AppendLine();
            }

            // Middle of string detection, no period
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"addr:street\"][\"addr:street\"=~/ {kvp.Key} /] {{");
                output.AppendLine($"assertNoMatch: \"way \\\"addr:street\\\"=Main {kvp.Value} East\";");
                output.AppendLine($"assertMatch: \"way \\\"addr:street\\\"=Main {kvp.Key} East\";");
                output.AppendLine($"throwWarning: tr(\"addr:street={{0}} contains ' {kvp.Key} ', should likely be expanded to {kvp.Value}\",\"{{0.value}}\");");
                output.AppendLine($"fixAdd: concat(\"addr:street=\", replace(tag(\"addr:street\"),\" {kvp.Key} \", \" {kvp.Value} \"));");
                output.AppendLine($"group: tr(\"addr:street contains {kvp.Key}, should likely be expanded to {kvp.Value}\");");
                output.AppendLine("}");
                output.AppendLine();
            }
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"name\"=~/ {kvp.Key} /].highway_name {{");
                output.AppendLine($"assertNoMatch: \"way \\\"name\\\"=Main {kvp.Value} East\";");
                output.AppendLine($"assertMatch: \"way \\\"name\\\"=Main {kvp.Key} East\";");
                output.AppendLine($"throwWarning: tr(\"Highway name contains {kvp.Key}, may need to be expanded to {kvp.Value}\");");
                output.AppendLine($"fixAdd: concat(\"name=\", replace(tag(\"name\"),\" {kvp.Key} \", \" {kvp.Value} \"));");
                output.AppendLine("}");
                output.AppendLine();
            }

            // Middle of string detection, period
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"addr:street\"][\"addr:street\"=~/ {kvp.Key}\\. /] {{");
                output.AppendLine($"assertNoMatch: \"way \\\"addr:street\\\"=Main {kvp.Value} East\";");
                output.AppendLine($"assertMatch: \"way \\\"addr:street\\\"=Main {kvp.Key} East\";");
                output.AppendLine($"throwWarning: tr(\"addr:street={{0}} contains ' {kvp.Key}. ', should likely be expanded to {kvp.Value}\",\"{{0.value}}\");");
                output.AppendLine($"fixAdd: concat(\"addr:street=\", replace(tag(\"addr:street\"),\" {kvp.Key}. \", \" {kvp.Value} \"));");
                output.AppendLine($"group: tr(\"addr:street contains {kvp.Key}, should likely be expanded to {kvp.Value}\");");
                output.AppendLine("}");
                output.AppendLine();
            }
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"name\"=~/ {kvp.Key}\\. /].highway_name {{");
                output.AppendLine($"assertNoMatch: \"way \\\"name\\\"=Main {kvp.Value} East\";");
                output.AppendLine($"assertMatch: \"way \\\"name\\\"=Main {kvp.Key} East\";");
                output.AppendLine($"throwWarning: tr(\"Highway name contains {kvp.Key}., may need to be expanded to {kvp.Value}\");");
                output.AppendLine($"fixAdd: concat(\"name=\", replace(tag(\"name\"),\" {kvp.Key}. \", \" {kvp.Value} \"));");
                output.AppendLine("}");
                output.AppendLine();
            }

            // Prefix, no period
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"addr:street\"][\"addr:street\"=~/^{kvp.Key} /] {{");
                output.AppendLine($"assertNoMatch: \"way \\\"addr:street\\\"={kvp.Value} Foo\";");
                output.AppendLine($"assertMatch: \"way \\\"addr:street\\\"={kvp.Key} Foo\";");
                output.AppendLine($"throwWarning: tr(\"addr:street={{0}} begins with {kvp.Key}, should likely be expanded to {kvp.Value}\",\"{{0.value}}\");");
                output.AppendLine($"fixAdd: concat(\"addr:street=\", \"{kvp.Value}\", substring(tag(\"addr:street\"), {kvp.Key.Length}));");
                output.AppendLine($"group: tr(\"addr:street begins with {kvp.Key}, may need to be expanded to {kvp.Value}\");");
                output.AppendLine("}");
                output.AppendLine();
            }
            foreach (var kvp in abbreviations)
            {
                output.AppendLine($"*[\"name\"=~/^{kvp.Key} /].highway_name {{");
                output.AppendLine($"assertNoMatch: \"way \\\"name\\\"={kvp.Value} Foo\";");
                output.AppendLine($"assertMatch: \"way \\\"name\\\"={kvp.Key} Foo\";");
                output.AppendLine($"throwWarning: tr(\"Highway name begins with {kvp.Key}, may need to be expanded to {kvp.Value}\");");
                output.AppendLine($"fixAdd: concat(\"name=\", \"{kvp.Value}\", substring(tag(\"name\"), {kvp.Key.Length}));");
                output.AppendLine("}");
                output.AppendLine();
            }

            File.WriteAllText(@"USStreetNameExpander.validator.mapcss", output.ToString());
        }
    }
}