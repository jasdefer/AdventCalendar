using AdventCalendarWebApp.Helper.TimeProvider;
using AdventCalendarWebApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace AdventCalendarWebApp.Pages._2021;

public class WikiArticleGuesserModel : PageModel
{
    public const int DefaultNumberOfHints = 5;
    public const int NumberOfHintsPerTry = 3;
    public static readonly IReadOnlyList<string> articles = new string[12]
    {
            "Michael_Schumacher",
            "Big_Ben",
            "Taylor_Swift",
            "swimming",
            "Rome",
            "Microsoft",
            "Japan",
            "Periodic table",
            "Barack_Obama",
            "Canada",
            "Computer",
            "Brush"
    };

    public static readonly IReadOnlyList<string> Hints = new string[12]
    {
            "Person",
            "Buildings and structures",
            "Person",
            "Activity",
            "Location",
            "Invention",
            "Location",
            "Invention",
            "Person",
            "Location",
            "Invention",
            "Tool",
    };

    public static IReadOnlyList<IReadOnlyList<string>> Words = new string[12][]
    {
            new string[]{ "third","positions","controversial","incidents","return","driver","skiing","was","tens","before","including","ability","final","australian","suffered","appearance","season","brief","drivers","runnerup","former","signed","unesco","sustained","whom","periods","collisions","ralf","series","championship","involved","galvanise","success","lewis","rehabilitation","brother","williams","december","until","dollars","title","rest","ahead","medically","pushing","injury","placed","career","competed","teams","maintains","mercedes","september","decided","limit","regimen","second","retirement","won","receive","hospital","win","millions","formula","consecutive","several","grenoble","karting","born","being","sixth","pioneering","breaking","during","titles","laps","seventh","singleseater","moved","damon","listen","amongst","around","german","unprecedented","privately","beginning","villeneuve","races","very","grand","five","broken","hamiltonwhile","enjoyed","further","four","first","records","fitness","accident","june","coma","has","retired","induced","brain","prix","racing","same","ferrari","umax","others","after","severe","benetton","left","subsequent","fastest","junior","projects","feat","pole","home","seven","jointrecord","wins","charity","younger","m�ael","made","struggling","hamilton","finishing","tied","finishes","sport","european","relocated","although","humanitarian","are","race","medical","later","since","car","treatment","university","twice","noted","held","donated","jacques","repeated","january","team","world","oneoff","hill","belgian","finish","jordan","podium","record","lausanne","ambassador","siblings","consecutively" },
            new string[]{ "named","style","films","set","installation","westminster","repainting","rose","schedule","until","fourfaced","short","original","has","grade","include","nations","scotland","oversaw","renamed","were","end","england","heritage","chime","long","minutes","london","backup","diameter","elizabeth","shot","victorian","each","jubilee","remembrance","used","held","modifications","among","works","part","thistle","celebrations","repairing","boxing","reglazing","lighting","base","although","mechanism","stands","tower","quarter","uses","ireland","united","shamrock","exceptions","icon","renovation","name","tonnes","side","such","square","clock","work","origin","before","question","past","mark","climb","open","are","five","towers","striking","ground","electric","nickname","years","steps","building","hour","caunt","shields","tons","chiming","world","measuring","eve","often","northern","augustus","tolls","upgrading","improvements","tiles","official","after","roof","lift","featuring","palace","prominent","refer","bell","level","belfry","adding","extended","few","establishing","tall","dials","august","parliamentary","neogothic","was","located","pugin","began","motor","great","hall","bells","queen","may","diamond","largest","kingdom","since","sir","site","british","symbols","benjamin","accurate","champion","silent","wales","heavyweight","designed","fouryear","listed","democracy","north","weighs","originally","four","anniversary","represented","recognised","frequently","completed","feet","sunday","unesco","cultural","leek"},
            new string[]{ "genres","deal","successor","inspired","acclaimed","rolling","industry","grammy","debut","red","masters","billboards","fearless","riaa","speak","songwriting","alison","association","look","influences","back","next","knew","folk","fame","country","ever","after","teardrops","nashville","artists","indie","blended","documentaries","having","willow","shake","america","third","long","expanded","critically","americana","pop","personal","advocacy","contract","included","media","mtv","trouble","urban","many","ownership","topfive","sonyatv","featured","wins","records","folklore","cardigan","experiences","fourth","rights","woman","decade","greatest","forbes","guitar","worldwide","radio","singersongwriter","awards","beyond","during","electronic","lover","celebrity","guinness","singer","influence","chart","charttopping","incorporating","mainstream","contained","influential","rock","american","singles","lists","expired","experimented","brit","said","space","released","sound","certified","eighth","studio","album","crossover","such","bestselling","are","billboard","songwriter","dispute","miss","sold","sessions","coverage","fully","pond","including","diamond","often","various","six","widespread","december","song","shouldve","career","received","million","recognized","never","songs","rose","need","spawned","recording","year","praise","critical","further","stones","video","world","five","explored","born","fifth","topten","signed","belong","ranked","moved","artist","machine","music","philanthropic","bad","three","transitioned","made","alternative","albums","emmy","synthpop","eponymous","times","electropop","award","include","story","named","reputation","publishing","appeared","calm","blank","international","narrative","mine","aspiring","second","elements","songwriters","was","womens","evermore","has","big","hot","accolades"},
            new string[]{ "activities","survival","compulsory","undertake","sport","person","rudimentary","summer","weeks","including","locomotion","selfpropulsion","curriculum","body","response","achieved","olympics","every","usually","locomotive","range","substance","local","through","competitions","part","educational","modern","top","humans","exercise","limbs","liquid","national","international","movement","features","recreational","breath","hold","public","water","birth","within","lessons","consistently","are","among","recreation","underwater","coordinated","countries","formalized" },
            new string[]{ "originated","bnl","along","beginning","middle","successively","centre","aimed","specialised","spans","nicholas","cinecitt�","activity","smallest","occupied","urbs","slowly","summer","political","programme","location","eventually","european","business","oil","longer","lazio","italian","set","centuries","until","many","millennia","firstever","ages","century","eur","involved","due","headquarters","union","marked","aeterna","often","limits","banks","sculptors","tibullus","making","urban","baroque","coherent","under","while","much","tim","centres","kingdom","unicredit","years","cultural","metropolitan","parliamentary","citt�","independent","district","residents","tourist","fell","regarded","such","called","founding","thirdmost","mythology","visited","famous","ovid","pursued","special","continuously","mediterranean","development","ifad","olympics","international","referred","artists","described","city","creating","movies","europe","fashion","was","early","imperial","fund","listed","population","ufm","seat","existing","citys","hills","settlement","organization","studios","major","capital","example","renowned","fao","eterna","leonardo","presence","latin","cities","popular","sometimes","papacy","metropolis","empire","eni","oldest","industry","host","country","pharmaceutical","world","states","wfp","painters","fall","dates","portion","first","artistic","style","both","countrys","mix","birthplace","hundred","history","region","agriculture","livy","capitale","listen","services","reason","has","four","named","neoclassicism","hosts","academy","west","boundaries","seven","popes","mundi","latium","throughout","third","populated","peninsula","agencies","destination","unesco","architectural","poet","sabines","almost","national","area","inhabited","heritage","within","historic","tiber","awardwinning","enel","located","secretariat","papal","masterpieces","financial","latins","renaissance","expression","caput","million","etruscans" },
            new string[]{ "ipo","game","range","enterprise","three","personal","replaced","dethroned","word","hololens","tablet","touchscreen","followed","acquisitions","laptops","reach","servers","visual","google","microcomputer","created","systems","sell","consumer","corporation","being","through","since","market","has","desktops","technologies","industry","office","wide","acquisition","big","explorer","first","skype","basic","development","unfolded","computing","gadgets","portmanteau","develop","envisioned","estimated","ballmer","later","worlds","computers","global","linkedin","rankings","products","scaled","tabs","employees","publicly","corporate","gates","web","founded","division","inc","ibm","cloud","related","mids","valuation","trillion","after","highest","becoming","five","entering","maker","computer","forming","initial","made","offering","increasingly","third","focused","suite","value","states","public","information","rose","reached","interpreters","produces","corporations","trilliondollar","billion","american","companys","took","was","search","diversified","among","amazon","msdos","shares","ranked","fortune","considered","cap","production","android","bill","hardware","number","ceo","acquiring","strategy","are","subsequent","devices","steve","facebook","electronics","world","although","known","technology","total","revenue","studio","best","danger","respectively","company","united","companies","mixed","majority","video","nokias","line","windows","xbox","reclaimed","back","move","azure","december","multinational","price","dominate","edge","april","helped","internet","instead","surface","browsers","thirdhighest","bing","mobile","rise","msn","may","lost","satya","share","lineup","valuable","traded","millionaires","nadella","launch","allen","valued","billionaires","flagship","earlier","reality","including","overall","software","system","operating","along","altair","position","apple","brand","compatible","services","consoles","largest","june","digital"},
            new string[]{ "paleolithic","encompasses","automotive","main","nobility","samurai","appears","northwest","adopted","including","threefourths","fleet","expectancies","came","art","shikoku","has","court","centurylong","member","reunified","end","world","mountainous","mention","taiwan","secondlargest","contributions","administrative","asia","decline","countrys","foreign","centuries","metropolitan","kingdoms","after","was","electronics","significant","eleventhmost","becoming","south","warrior","based","sapporo","fire","global","atomic","power","prefectures","led","modernization","open","war","video","terrain","divided","gdp","pursued","chronicle","leader","empire","constitution","yokohama","pacific","record","heianky","tokyo","industries","extends","constitutional","civil","between","seven","imperial","million","archipelago","concentrating","during","axis","military","forced","ocean","residents","economic","worlds","strongest","numerous","selfdefense","fukuoka","suffering","osaka","country","sevenyear","around","inhabited","highest","under","upper","became","forces","prominent","legislature","part","miracle","international","largest","since","kyoto","regions","right","experiencing","eight","island","city","human","declare","beginning","fourthlargest","high","nippon","parliamentary","include","shogunate","unified","century","thirdlargest","comic","populated","more","science","great","cuisine","kyushu","technology","okinawa","nagoya","maintains","defeat","enforced","population","square","emperor","westernmodeled","invaded","industrialization","okhotsk","period","game","program","popular","meiji","urbanized","located","renounced","shgun","animation","sea","formally","very","major","cities","restoration","west","entered","militaries","culture","class","capital","kobe","united","lords","five","occupation","toward","north","kilometers","nations","monarchy","national","trade","political","nihon","written","greater","ppp","spans","plains","diet","ring","bicameral","hokkaido","known","nominal","islands","experienced" },
            new string[]{ "pioneering","periods","still","needed","internal","physics","optimal","underlying","scientific","column","discovery","reached","confirm","yet","todays","surrendering","configurations","illuminated","dependence","mendeleev","used","table","reason","patterns","fact","missing","fblock","heaviest","progress","down","atomic","characterisation","central","left","four","evolve","formulated","positioned","regarding","positions","electron","dmitri","atom","representations","own","sciences","periodic","indispensable","similar","formulation","match","actinides","form","accepted","rectangular","first","exist","discussion","generally","fundamental","right","electrons","display","century","recognisably","number","rather","numbers","run","explained","period","gaps","mass","tabular","through","part","known","late","keeping","stretch","dblock","seaborgs","opposite","recognized","group","show","increasing","completing","become","blocks","modern","law","work","alternative","direction","chemistry","necessary","graphic","elements","nonmetallic","character","many","icon","seven","trends","across","unknown","laboratory","are","properties","states","rows","correctly","continues","chemical","early","successfully","roughly","chemist","divided","groups","areas","beyond","quantum","continue","science","synthesise","mechanics","same","glenn","russian","structure","called","region","far","today","widely","predict","exhibit","metallic","were","atoms","further","seen","columns","characteristics","whether","nature","was" },
            new string[]{ "sotomayor","change","campaign","ban","filed","bestselling","affordable","care","elected","remained","politician","deadly","relief","russia","war","academic","regarded","consumer","ruled","control","combat","elementary","audacity","landslide","nine","aca","ended","without","repeal","review","favorably","turning","process","iran","nominated","initiated","was","romney","strike","national","august","hussein","gun","nomination","sanctions","oversaw","teaching","doddfrank","inaugurated","convention","alongside","act","economy","brokered","prize","partisan","active","briefs","discussions","court","abroad","where","mitt","climate","first","include","apologized","months","enrolled","justices","senator","again","confirmed","americans","debt","peace","following","gains","attention","tell","stimuli","vote","generally","dont","although","executive","normalized","never","campaigning","constitutional","received","promoted","job","nominee","immigration","plan","appearing","administration","tax","reauthorization","community","author","winning","elena","later","concerning","land","urged","united","afghanistan","political","global","general","security","january","district","state","elective","policy","recession","republicanmajority","many","protection","retired","address","joint","defeating","killing","significantly","osama","isil","warming","military","gaddafi","harvard","lengthy","organizer","ohbahm","taxpayer","deal","hook","election","legalized","since","joe","beginning","running","syria","operation","assault","weapons","africanamerican","opponent","involvement","after","year","including","reside","school","frequently","win","relations","contributing","advocated","airstrike","march","hoosayn","graduating","windsor","overthrow","named","inclusion","improved","unconstitutional","among","presidential","invasion","november","levels","sonia","worked","health","mate","mccain","terms","second","issued","civil","were","hospital","keynote","presidents","down","mcconnell","treaty","shooting","continues"},
            new string[]{ "act","ranks","among","parliamentary","pacific","has","institutions","armed","multicultural","human","metropolitan","widening","ability","international","part","civil","many","along","ceded","parliament","secondlargest","formed","extend","country","conflicts","freedom","head","increasing","government","highest","appointed","later","total","level","command","british","western","globally","commonsand","federal","immigration","highly","product","transparency","france","was","impact","constitutional","relationship","significant","years","northward","confidence","toronto","square","upon","measurements","countrys","group","virtue","union","united","nearly","tenthlargest","nominal","representing","nations","arctic","kingdom","asiapacific","forum","several","council","french","percapita","beginning","three","development","statute","holds","culminated","prime","internationale","resources","area","major","indigenous","westminster","explored","began","oecd","dominion","provinces","peoples","organization","elected","north","highlighted","chiefly","thousands","quality","ministerwho","tradition","consequence","autonomy","networks","areas","are","century","wto","income","officially","colonies","longest","office","had","developed","culture","kilometres","severed","economy","long","various","atlantic","cooperation","serves","million","ten","continuously","inhabited","countries","covering","seventeenthhighest","bilingual","vancouver","expeditions","ethnically","commonwealth","land","advanced","making","miles","binational","groupings","abundant","welldeveloped","largescale","house","america","dependence","accretion","border","confederation","trade","southern","natural","ottawa","states","life","index","vestiges","education","governor","state","world","legal","largest","nato","organisation","monarchy","relying","ranking","through","francophonie","democracy","stretching","intergovernmental","diverse","liberties","realm","territories","process","worlds","settled","four","including","coast","monarch","ocean","capital","montreal","sixteenthhighest","economic","general","american" },
            new string[]{ "built","term","microprocessor","looms","form","design","conventionally","mice","are","since","developed","versatility","source","dramatically","typically","included","mobile","patterns","complete","early","automatically","machine","generalpurpose","predicted","specialized","aided","analog","speed","late","remote","includes","leading","during","specialpurpose","main","include","transistors","were","element","sequencing","least","increasing","personal","users","memory","sophisticated","cpu","type","control","perform","world","together","enable","both","did","calculating","internet","peripheral","smartphones","monitor","response","mosfet","consists","generic","doing","result","devices","stored","systems","logical","joystick","times","digital","mechanical","century","touchscreen","hardware","saved","cluster","revolution","ovens","manual","meant","arithmetic","broad","war","programmed","ancient","inputoutput","products","counts","first","machines","robots","people","external","linked","function","simple","information","chip","power","used","electronic","along","change","carries","wide","automate","chips","unit","modern","transistor","programs","sequences","industrial","consumer","central","known","order","technologies","network","microwave","law","integrated","operation","output","tasks","range","full","functions","allow","processing","semiconductor","carry","guiding","instruments","software","millions","retrieved","operating","sera","refer","equipment","rapid","etc","long","moores","followed","monolithic","screens","tedious","group","electrical","printers","keyboards","hundreds","more","needed","centuries","ever","circuit","operations","abacus","system","controls","calculations","factory","mos","sets","may","such","input","pace","siliconbased","links" },
            new string[]{ "hair","are","many","chosen","handle","common","withstand","such","dozen","affixed","versatile","depending","chemicals","either","varieties","generally","hazards","average","filaments","household","tool","several","purposes","basic","used","gripped","block","orientation","wire","may","contain","perpendicular","today","grooming","material","during","cleaning","bristles","corrosive","intended","surface","heat","finishing","abrasion","consists","both","parallel","painting","tools" },
    };

    private readonly DayValidation dayValidation;
    private readonly AzureHelper azureHelper;
    private readonly IConfiguration configuration;
    private readonly ITimeProvider timeProvider;

    public ValidationState ValidationState { get; private set; } = ValidationState.NotValidated;

    [BindProperty]
    public string Answer { get; set; }
    [BindProperty]
    public int NumberOfHints { get; set; }
    [BindProperty]
    public int Day { get; set; }
    [BindProperty]
    public int NumberOfGuesses { get; set; }
    [BindProperty]
    public DateTime StartOfGuessing { get; set; }

    public int Index => Day / 2;
    public TimeSpan SolveDuration => timeProvider.Now() - StartOfGuessing;

    public WikiArticleGuesserModel(DayValidation dayValidation,
        AzureHelper azureHelper,
        IConfiguration configuration,
        ITimeProvider timeProvider)
    {
        this.dayValidation = dayValidation;
        this.azureHelper = azureHelper;
        this.configuration = configuration;
        this.timeProvider = timeProvider;
    }

    public async Task<IActionResult> OnGet(int day,
        int numberOfHints = DefaultNumberOfHints,
        int numberOfGuesses = 0,
        string answer = null,
        DateTime? startOfGuessing = null)
    {
        Day = day;
        var hasAccess = dayValidation.HasAccess2021(day);
        if (day % 2 == 0 || !hasAccess)
        {
            return NotFound();
        }
        NumberOfHints = Math.Max(DefaultNumberOfHints, Math.Min(numberOfHints, Words[Index].Count - 1));
        NumberOfGuesses = Math.Max(0, numberOfGuesses);
        StartOfGuessing = startOfGuessing ?? timeProvider.Now();
        if (string.IsNullOrEmpty(answer))
        {
            return Page();
        }
        NumberOfGuesses++;

        if (answer.Trim().Equals(articles[Index].Replace("_", " "), StringComparison.InvariantCultureIgnoreCase))
        {
            ValidationState = ValidationState.Correct;
        }
        else
        {
            ValidationState = ValidationState.Incorrect;
            NumberOfHints += NumberOfHintsPerTry;
            NumberOfHints = Math.Min(NumberOfHints, Words[Index].Count);
        }

        Answer = answer;
        await LogWikiArticleGuess();

        return Page();
    }

    private async Task LogWikiArticleGuess()
    {
        var userId = HttpContext.GetOrCreateUserId();
        var wikiArticleGuess = new WikiArticleGuess()
        {
            Day = Day,
            Guess = Answer,
            PartitionKey = userId,
            RowKey = Guid.NewGuid().ToString(),
            UserId = userId,
            NumberOfGuesses = NumberOfGuesses,
            NumberOfHints = NumberOfHints,
            GuessTimestamp = timeProvider.Now(),
            IsCorrect = ValidationState == ValidationState.Correct,
            SolveDurationSeconds = SolveDuration.TotalSeconds
        };
        await azureHelper.AddObjectAsync(configuration["StorageData:2021WikiArticleGuessesTableName"], wikiArticleGuess);
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("WikiArticleGuesser",
            new
            {
                day = Day,
                numberOfHints = NumberOfHints,
                numberOfGuesses = NumberOfGuesses,
                answer = Answer,
                startOfGuessing = StartOfGuessing,
            });
    }
}
