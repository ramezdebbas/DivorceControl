using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Reasons & Directions",
                    "Reasons & Directions",
                    "Assets/Images/10.jpg",
                    "Divorce (or the dissolution of marriage) is the final termination of a marital union, cancelling the legal duties and responsibilities of marriage and dissolving the bonds of matrimony between the parties (unlike annulment, which declares the marriage null and void). Divorce laws vary considerably around the world, but in most countries it requires the sanction of a court or other authority in a legal process.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Why Divorce?",
                    "Divorce (or the dissolution of marriage) is the final termination of a marital union, cancelling the legal duties and responsibilities of marriage and dissolving the bonds of matrimony between the parties (unlike annulment, which declares the marriage null and void). Divorce laws vary considerably around the world, but in most countries it requires the sanction of a court or other authority in a legal process.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nDivorce (or the dissolution of marriage) is the final termination of a marital union, cancelling the legal duties and responsibilities of marriage and dissolving the bonds of matrimony between the parties (unlike annulment, which declares the marriage null and void). Divorce laws vary considerably around the world, but in most countries it requires the sanction of a court or other authority in a legal process. The legal process of divorce may also involve issues of alimony (spousal support), child custody, child support, distribution of property, and division of debt. In most countries monogamy is required by law, so divorce allows each former partner to marry another; where polygyny is legal but polyandry is not, divorce allows the woman to marry a new husband.\n\nLike every major life change, divorce can be a stressful experience. It affects finances, living arrangements, household jobs, schedules and more. If the family includes children, they may be deeply affected.\n\nA number of countries have since the 1970s legislated to permit divorce, including Italy (1970), Portugal (1975), Spain (1981), Ireland (1996) and Malta (2011). Today, the only countries which do not allow divorce are the Philippines (though Muslims have the right to divorce) and the Vatican City, an ecclesiastical state, which has no procedure for divorce.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Why Divorce?", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Divorce Control" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Divorce and Relationships",
                     "Research done at Northern Illinois University on Family and Child Studies suggests that divorce can have a positive effect on families due to less conflict in the home. There are, however, many instances where the parent-child relationship may suffer due to divorce. Financial support is many times lost when an adult goes through a divorce",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nResearch done at Northern Illinois University on Family and Child Studies suggests that divorce can have a positive effect on families due to less conflict in the home. There are, however, many instances where the parent-child relationship may suffer due to divorce. Financial support is many times lost when an adult goes through a divorce. The adult may be obligated to obtain additional work to maintain financial stability. In turn, this can lead to a negative relationship between the parent and child. The relationship may suffer due to lack of attention towards the child as well as minimal parental supervision\n\nStudies have also shown that parental skills decrease after a divorce occurs; however, this effect is only a temporary change. A number of researchers have shown that a disequilibrium, including diminished parenting skills, occurs in the year following the divorce but that by two years after the divorce re-stabilization has occurred and parenting skills have improved\n\nIn a study done by the American Psychological Association on a parents’ relocation after a divorce, found that a move is a long-term affect on children. In the first study done amongst 2,000 college students on the effects of parental relocation relating to the well being if their children after divorce, researchers found major differences. In divorced families where one parent moved, the students received less financial support from their parents compared with divorced families where neither parent moved. These findings also imply other negative outcomes for these students such as more distress related to the divorce and did not feel a sense of emotional support from their parents. Although the data suggests negative outcomes for these students whose parents relocate after divorce, there is not enough research that can alone prove the overall well-being of the child[12] A newer study in the Journal of Family Psychology found that parents who move more than an hour away from their children after a divorce are much less well off than those parents who stayed in the same location",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Divorce and Relationships", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Divorce Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Effects of divorce on children",
                     "Sociologists know little about the effects on children younger than two or three years of age. Children from age range from 3–5 years old may often mistake the divorce of their parents as their own fault. Older children experience feelings of anger, grief, and embarrassment.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nSociologists know little about the effects on children younger than two or three years of age. Children from age range from 3–5 years old may often mistake the divorce of their parents as their own fault. Older children experience feelings of anger, grief, and embarrassment.People think that post-separation parenting is easy - in fact, it is exceedingly difficult, and as a rule of thumb my experience is that the more intelligent the parent, the more intractable the dispute There is nothing worse, for most children, than for their parents to denigrate each other Parents simply do not realize the damage they do to their children by the battles they wage over them. Separating parents rarely behave reasonably, although they always believe that they are doing so, and that the other party is behaving unreasonably. - Sir Nicholas Scott(President of the family division of the High Court)\n\nAlthough not the intention of most parents, putting children in the middle of conflict is particularly detrimental. Examples of this are: asking children to carry messages between parents, grilling children about the other parent’s activities, telling children the other parent does not love them, and putting the other parent down in front of the children. Poorly managed conflict between parents increases children’s risk of behavior problems, depression, substance abuse and dependence, poor social skills, and poor academic performance. Fortunately, there are approaches by which divorce professionals can help parents reduce conflict. Options include mediation, collaborative divorce, coparent counseling, and parenting coordination.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Effects of divorce on children", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Divorce Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Secularisation",
                     "After the Reformation, marriage came to be considered a civil contract in the non-Catholic regions, and on that basis civil authorities gradually asserted their power to decree a divortium a vinculo matrimonii, or divorce from all the bonds of marriage",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAfter the Reformation, marriage came to be considered a civil contract in the non-Catholic regions, and on that basis civil authorities gradually asserted their power to decree a divortium a vinculo matrimonii, or divorce from all the bonds of marriage. Since no precedents existed defining the circumstances under which marriage could be dissolved, civil courts heavily relied on the previous determinations of the ecclesiastic courts and freely adopted the requirements set down by those courts. As the civil courts assumed the power to dissolve marriages, courts still strictly construed the circumstances under which they would grant a divorce,[47] and considered divorce to be contrary to public policy. Because divorce was considered to be against the public interest, civil courts refused to grant a divorce if evidence revealed any hint of complicity between the husband and wife to divorce, or if they attempted to manufacture grounds for a divorce. Divorce was granted only because one party to the marriage had violated a sacred vow to the innocent spouse. If both husband and wife were guilty, neither would be allowed to escape the bonds of marriage.\n\nEventually, the idea that a marriage could be dissolved in cases in which one of the parties violated the sacred vow gradually allowed expansion of the grounds upon which divorce could be granted from those grounds which existed at the time of the marriage to grounds which occurred after the marriage, but which exemplified violation of that vow, such as abandonment, adultery, or extreme cruelty",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Secularisation", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Divorce Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Religion and Divorce",
                     "In some countries (commonly in Europe and North America), the government defines and administers marriages and divorces. While ceremonies may be performed by religious officials on behalf of the state, a civil marriage and thus, civil divorce (without the involvement of a religion) is also possible.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIn some countries (commonly in Europe and North America), the government defines and administers marriages and divorces. While ceremonies may be performed by religious officials on behalf of the state, a civil marriage and thus, civil divorce (without the involvement of a religion) is also possible. Due to differing standards and procedures, a couple can be legally unmarried, married, or divorced by the state's definition, but have a different status as defined by a religious order. Other countries use religious law to administer marriages and divorces, eliminating this distinction. In these cases, religious officials are generally responsible for interpretation and implementation.\n\nIslam allows divorce, and it can be initiated by either the husband or the wife. However, the initiations are subject to certain conditions and waiting periods, which are meant to force the initiating party to reconsider.\n\nDharmic religions do not allow divorce.[citation needed] Christian views of divorce vary, with Catholic teaching allowing only annulment, but most other denominations discouraging but allowing divorce. Jewish views of divorce differ, with Reform Judaism considering civil divorces adequate. Conservative and Orthodox Judaism require that the husband grant his wife a divorce in the form of a get.\n\nThe Millet System, where each religious group regulates its own marriages and divorces, is still present in varying degrees in some post−Ottoman countries like Iraq, Syria, Jordan, Lebanon, Israel, the Palestinian Authority, Egypt, and Greece. Several countries use sharia (Islamic law) to administrate marriages and divorces for Muslims. Thus, Marriage in Israel is administered separately by each religious community (Jews, Christians, Muslims, and Druze), and there is no provision for interfaith marriages other than marrying in another country. For Jews, marriage and divorce are administered by Orthodox rabbis. Partners can file for divorce either in rabbinical court or Israeli civil court.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Religion and Divorce", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Divorce Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Gender and Divorce",
                     "According to a study published in the American Law and Economics Review, women have filed slightly more than two-thirds of divorce cases in the United States. There is some variation among states, and the numbers have also varied over time, with about 60% of filings by women in most of the 19th century.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAccording to a study published in the American Law and Economics Review, women have filed slightly more than two-thirds of divorce cases in the United States.There is some variation among states, and the numbers have also varied over time, with about 60% of filings by women in most of the 19th century, and over 70% by women in some states just after no-fault divorce was introduced, according to the paper. Evidence is given that among college-educated couples, the percentage of divorces initiated by women is approximately 90%.\n\nA study has found that White female-Black male and White female-Asian male marriages are more prone to divorce than White-White pairings.[30] Conversely, unions between White males and non-White females (and between Hispanics and non-Hispanic persons) have similar or lower risks of divorce than White-White marriages.\n\nRegarding divorce settlements, according to the 2004 Grant Thornton survey in the UK, women obtained a better or considerably better settlement than men in 60% of cases. In 30% of cases the assets were split 50-50, and in only 10% of cases did men achieve better settlements (down from 24% the previous year). The report concluded that the percentage of shared residence orders would need to increase in order for more equitable financial divisions to become the norm.\n\nSome jurisdictions give unequal rights to men and women when filing for divorce.\n\nFor couples to Conservative or Orthodox Jewish law (which by Israeli civil law includes all Jews in Israel), the husband must grant his wife a divorce through a document called a get. If the man refuses, the woman can appeal to a court or the community to pressure the husband. A woman whose husband refuses to grant the get or who is missing is called an agunah, is still married, and therefore cannot remarry. Under Orthodox law, children of an extramarital affair involving a married Jewish woman are considered mamzerim (illegitimate) and cannot marry non-mamzerim.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Gender and Divorce", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Divorce Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Infidelity",
                     "If there is one thing that can be difficult to forgive in a relationship, much less a marriage, that would be having an affair. Regaining trust is far from being easy, and once this happens, the space in bed can become as wide as an ocean. But based on some studies, data showed that women are more forgiving compared to men.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIf there is one thing that can be difficult to forgive in a relationship, much less a marriage, that would be having an affair. Regaining trust is far from being easy, and once this happens, the space in bed can become as wide as an ocean. But based on some studies, data showed that women are more forgiving compared to men.\n\nA violation of trust in this regards can be absolutely devastating to the person it happens to. The ability to trust again can take a very long time if ever.\n\nMany couples try to make it work after an affair when children are involved. In this case, the two adults in the relationship must put the needs of the children ahead of their own. This is very difficult in a modern society where the media is constantly telling us to be self-focused and self-gratifying.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Infidelity", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Divorce Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Financial Problems",
                     "The present state of economy can greatly impact the lives of married individuals. This can also produce tons of pressure, enough that marriages can break in just a single snap.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nThe present state of economy can greatly impact the lives of married individuals. This can also produce tons of pressure, enough that marriages can break in just a single snap. This is among the saddest reasons for divorce, but it really happens, more often than the others in fact.\n\nFinancial pressures can have a negative affect on marriage in more ways than one. Lack of money or means can have an affect on the confidence and or the identity of one of the partners in a marriage. It can threaten feeling of safety and security to those who are dependants.\n\nFor the financial providers, having financial problems can cause many stress related health problems such as: high blood pressure, depression & anxiety, and insomnia. Money issues can certainly be a breaking point for some marriages.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Financial Problems", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Divorce Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Lack of Communication",
                     "Communication lines do not just break down overnight. It might take years, particularly when hurtful remarks have been exchanged by both parties.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nCommunication lines do not just break down overnight. It might take years, particularly when hurtful remarks have been exchanged by both parties. More often than not, the wounding comments are set aside but still, they pile up through the years, waiting to go over the brink.\n\nAre you able to talk about things with your partner or do you save it up for later? Do you or your partner like to keep score in order to have material in a verbal showdown?\n\nNegative communication can be just as damaging to a marriage as much as lack of communication. Lack of communication can be seen as indifference and that is just as devastating as anger and negativity.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Lack of Communication", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Divorce Control" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Abuse",
                     "It is not just about the physical type of abuse because this can also take various forms, such as intimidation, belittling, psychological, and emotional abuses.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIt is not just about the physical type of abuse because this can also take various forms, such as intimidation, belittling, psychological, and emotional abuses.Neglect is also a form of abuse. Feelings or being marginalized or not being of value to a marital partner can have devastating effects on a person's self-esteem and self-worth.\n\nAbuse can take many forms and most of the time, the one being abused is in denial of the situation. They may try to wait it out thinking that things will get better someday.\n\nMany times it's friends or family that are the first to notice changes in behavior of the person being abused. Men and women can both be abused in a marriage so this is not a gender specific issue.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Reasons & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Abuse", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Divorce Control" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
