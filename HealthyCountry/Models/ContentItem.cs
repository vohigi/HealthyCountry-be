using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace HealthyCountry.Models
{

    public interface IContentItem : IEquatable<IContentItem>
    {
        string Name { get; set; }

        bool IsHidden { get; set; }
    }

    internal class ContentItemNameAttribute : Attribute
    {
        internal ContentItemNameAttribute(string name)
        {
            Name = name;
        }

        internal string Name { get; private set; }
    }

    public abstract class HiddenContent<TBuilder> : IContentItem
        where TBuilder : HiddenContent<TBuilder>
    {
        public HiddenContent()
        {
            _instance = (TBuilder) this;
        }

        private readonly TBuilder _instance;

        public TBuilder Hide()
        {
            IsHidden = true;
            return _instance;
        }

        public TBuilder Hide(Func<TBuilder, bool> predicate)
        {
            if (predicate(_instance)) IsHidden = true;

            return _instance;
        }

        public abstract bool Equals(IContentItem other);

        public string Name { get; set; }
        public bool IsHidden { get; set; }
    }

    [ContentItemName("Field")]
    public class FieldContent : HiddenContent<FieldContent>, IEquatable<FieldContent>
    {
        public FieldContent()
        {

        }

        public FieldContent(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Value { get; set; }

        public bool Equals(FieldContent other)
        {
            if (other == null) return false;

            return Name == other.Name && Value == other.Value;
        }

        public override bool Equals(IContentItem other)
        {
            if (!(other is FieldContent)) return false;

            return Equals((FieldContent) other);
        }

        public override int GetHashCode()
        {
            return new {Name, Value}.GetHashCode();
        }
    }

    [ContentItemName("Table")]
    public class TableContent : HiddenContent<TableContent>, IContentItem, IEquatable<TableContent>
    {
        public ICollection<TableRowContent> Rows { get; set; }

        public IEnumerable<string> FieldNames
        {
            get { return Rows?.SelectMany(r => r.FieldNames).Distinct().ToList() ?? new List<string>(); }
        }

        #region ctors

        public TableContent()
        {

        }

        public TableContent(string name)
        {
            Name = name;
            Rows = new List<TableRowContent>();
        }

        public TableContent(string name, IEnumerable<TableRowContent> rows)
            : this(name)
        {
            Rows = rows.ToList();
        }

        public TableContent(string name, params TableRowContent[] rows)
            : this(name)
        {
            Rows = rows.ToList();
        }

        #endregion

        #region Fluent

        public static TableContent Create(string name, params TableRowContent[] rows)
        {
            return new TableContent(name, rows);
        }

        public static TableContent Create(string name, List<TableRowContent> rows)
        {
            return new TableContent(name, rows);
        }

        public TableContent AddRow(params IContentItem[] contentItems)
        {
            if (Rows == null) Rows = new List<TableRowContent>();

            Rows.Add(new TableRowContent(contentItems));
            return this;
        }

        #endregion

        #region Equals

        public bool Equals(TableContent other)
        {
            if (other == null) return false;

            return Name.Equals(other.Name) &&
                   Rows.SequenceEqual(other.Rows);
        }

        public override bool Equals(IContentItem other)
        {
            if (!(other is TableContent)) return false;

            return Equals((TableContent) other);
        }

        public override int GetHashCode()
        {
            var hc = 0;
            if (Rows != null)
                hc = Rows.Aggregate(hc, (current, p) => current ^ p.GetHashCode());

            return new {Name, hc}.GetHashCode();
        }

        #endregion
    }
    [JsonObject]
    public class Container : IEnumerable<IContentItem>, IEquatable<Container>
	{
		public Container()
		{

                Repeats = new List<RepeatContent>();
                Lists = new List<ListContent>();
				Tables = new List<TableContent>();
				Fields = new List<FieldContent>();
				
            
		}
		public Container(params IContentItem[] contentItems)
		{
			if (contentItems != null)
			{
                Repeats = contentItems.OfType<RepeatContent>().ToList();
                Lists = contentItems.OfType<ListContent>().ToList();
				Tables = contentItems.OfType<TableContent>().ToList();
				Fields = contentItems.OfType<FieldContent>().ToList();
			}
		}

		public IEnumerable<IContentItem> All
		{
			get
			{
				var result = new List<IContentItem>();

                if (Repeats != null) result = result.Concat(Repeats).ToList();
                if (Tables != null) result = result.Concat(Tables).ToList();
				if (Lists != null) result = result.Concat(Lists).ToList();
				if (Fields != null) result = result.Concat(Fields).ToList();

				return result;
			}
		}

        public ICollection<RepeatContent> Repeats { get; set; }

        public ICollection<TableContent> Tables { get; set; }

		public ICollection<ListContent> Lists { get; set; }

		public ICollection<FieldContent> Fields { get; set; }

		public IContentItem GetContentItem(string name)
        {
	        return All.FirstOrDefault(t => t.Name == name);
        }
		[JsonIgnore]
		public IEnumerable<string> FieldNames
		{
			get
			{
                var repeatsFieldNames = Repeats == null
                    ? new List<string>()
                    : Repeats.Select(t => t.Name)
                        .Concat(Repeats.SelectMany(t => t.Items.SelectMany(r => r.FieldNames)));

                var tablesFieldNames = Tables == null
					? new List<string>()
					: Tables.Select(t => t.Name)
						.Concat(Tables.SelectMany(t => t.Rows.SelectMany(r => r.FieldNames)));

				var listsFieldNames = Lists == null
							? new List<string>()
							: Lists.Select(l => l.Name)
								.Concat(Lists.SelectMany(l => l.FieldNames));
				

				var fieldNames = Fields == null ? new List<string>() : Fields.Select(f => f.Name);

				return repeatsFieldNames
                    .Concat(tablesFieldNames)
                    .Concat(listsFieldNames)
                    .Concat(fieldNames);
			}
		}

		#region Fluent
		public Container AddField(string name, string value)
		{
			if (Fields == null) Fields = new List<FieldContent>();

			Fields.Add(new FieldContent(name, value));
			return this;
		}

		public Container AddTable(TableContent table)
		{
			if (Tables == null) Tables = new List<TableContent>();

			Tables.Add(table);
			return this;
		}
		public Container AddList(ListContent list)
		{
			if (Lists == null) Lists = new List<ListContent>();

			Lists.Add(list);
			return this;
		}
		#endregion

		#region IEnumerable
		public IEnumerator<IContentItem> GetEnumerator()
		{
			return All.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

		#region Equals
		public bool Equals(Container other)
		{
			if (other == null) return false;

			return All.SequenceEqual(other);
		}

		public override int GetHashCode()
		{
			var hc = 0;
			
			hc = All.Aggregate(hc, (current, p) => current ^ p.GetHashCode());

			return hc;
		}
		#endregion
	}
    public class TableRowContent:Container, IEquatable<TableRowContent>
    {
        public TableRowContent()
        {
            
        }

        public TableRowContent(params IContentItem[] contentItems)
            : base(contentItems)
        {
			
        }

        public TableRowContent(List<FieldContent> fields)
        {
            Fields = fields;
        }

        #region Equals

        public bool Equals(TableRowContent other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
    public class Content : Container, IEquatable<Content>
    {
	    public Content()
	    {
	    }
	    public Content(params IContentItem[] contentItems):base(contentItems)
	    {
	    }

	    public bool Equals(Content other)
	    {
		    return base.Equals(other);
	    }

	    public override int GetHashCode()
	    {
		    return base.GetHashCode();
	    }
    }
    [ContentItemName("List")]
	public class ListContent : HiddenContent<ListContent>, IEquatable<ListContent>
	{
	    public ICollection<ListItemContent> Items { get; set; }

		public IEnumerable<string> FieldNames => GetFieldNames(Items) ?? new List<string>();

	    #region ctors

		public ListContent()
        {
            
        }

        public ListContent(string name)
        {
            Name = name;
        }

		public ListContent(string name, IEnumerable<ListItemContent> items)
            : this(name)
        {
            Items = items.ToList();
        }

		public ListContent(string name, params ListItemContent[] items)
            : this(name)
        {
			Items = items.ToList();
        }

		#endregion

		#region Fluent

		public static ListContent Create(string name, params ListItemContent[] items)
        {
			return new ListContent(name, items);
        }

		public static ListContent Create(string name, IEnumerable<ListItemContent> items)
        {
			return new ListContent(name, items);
        }

		public ListContent AddItem(ListItemContent item)
		{
			if (Items == null) Items = new Collection<ListItemContent>();
			Items.Add(item);
			return this;
		}

		public ListContent AddItem(params IContentItem[] contentItems)
		{
			if (Items == null) Items = new Collection<ListItemContent>();
			Items.Add(new ListItemContent(contentItems));
			return this;
		}

        #endregion

        #region IContentItem implementation

        private List<string> GetFieldNames(IEnumerable<ListItemContent> items)
		{
			var result = new List<string>();
			if (items == null) return null;

			foreach (var item in items)
			{
				if (item != null)
				{
					foreach (var fieldName in item.FieldNames.Where(fieldName => !result.Contains(fieldName)))
					{
						result.Add(fieldName);
					}

					if (item.NestedFields != null)
					{
						var listItem = item;
						if (listItem.NestedFields != null)
						{
							var nestedFieldNames = GetFieldNames(listItem.NestedFields);
							foreach (var nestedFieldName in nestedFieldNames)
							{
								if (!result.Contains(nestedFieldName))
									result.Add(nestedFieldName);
							}
						}
					}

				}
			}			
			return result;
		}

		#endregion

		#region Equals

		public bool Equals(ListContent other)
		{
			if (other == null) return false;

            return Name.Equals(other.Name) && 
                FieldNames.SequenceEqual(other.FieldNames);
		}

		public override bool Equals(IContentItem other)
		{
			if (!(other is ListContent)) return false;

			return Equals((ListContent)other);
		}

		public override int GetHashCode()
		{
			var hc = 0;
			if (Items != null)
				hc = Items.Aggregate(hc, (current, p) => current ^ p.GetHashCode());
			
			return new { Name, hc }.GetHashCode();
		}

		#endregion
	}
	public class ListItemContent : Container, IEquatable<ListItemContent>
	{
		public ICollection<ListItemContent> NestedFields { get; set; }

		#region ctors

		public ListItemContent()
		{
			
		}

		public ListItemContent(params IContentItem[] contentItems):base(contentItems)
        {
            
        }

		public ListItemContent(IEnumerable<ListItemContent> nestedfields, params IContentItem[] contentItems)
			: base(contentItems)
		{
			NestedFields = nestedfields.ToList();
		}

		public ListItemContent(string name, string value)
		{
			Fields = new List<FieldContent> {new FieldContent {Name = name, Value = value}};
			NestedFields = new List<ListItemContent>();
		}
		
		public ListItemContent(string name, string value, IEnumerable<ListItemContent> nestedfields)
		{
			Fields = new List<FieldContent>{new FieldContent{Name = name, Value = value}};
			NestedFields = nestedfields.ToList();
		}

		public ListItemContent(string name, string value, params ListItemContent[] nestedfields)
		{
			Fields = new List<FieldContent> {new FieldContent {Name = name, Value = value}};
			NestedFields = nestedfields.ToList();

		}

		#endregion

		#region Fluent

		public static ListItemContent Create(string name, string value, params ListItemContent[] nestedfields)
		{
			return new ListItemContent(name, value, nestedfields);
		}

		public static ListItemContent Create(string name, string value, List<ListItemContent> nestedfields)
		{
			return new ListItemContent(name, value, nestedfields);
		}
		public new ListItemContent AddField(string name, string value)
		{
			return (ListItemContent)base.AddField(name, value);
		}

		public new ListItemContent AddTable(TableContent table)
		{
			return (ListItemContent)base.AddTable(table);
		}
		
		public new ListItemContent AddList(ListContent list)
		{
			return (ListItemContent)base.AddList(list);
		}
		
		public ListItemContent AddNestedItem(ListItemContent nestedItem)
		{
			if (NestedFields == null) NestedFields = new List<ListItemContent>();

			NestedFields.Add(nestedItem);
			return this;
		}

		public ListItemContent AddNestedItem(IContentItem nestedItem)
		{
			if (NestedFields == null) NestedFields = new List<ListItemContent>();

			NestedFields.Add(new ListItemContent(nestedItem));
			return this;
		}

		#endregion

		#region Equals
		public bool Equals(ListItemContent other)
		{
			if (other == null) return false;

			var equals = base.Equals(other);
	
			if (NestedFields != null)
				return equals && NestedFields.SequenceEqual(other.NestedFields);

			return equals;
		}
		public override int GetHashCode()
		{
			var nestedHc = 0;
			
			nestedHc = NestedFields.Aggregate(nestedHc, (current, p) => current ^ p.GetHashCode());
			var baseHc = base.GetHashCode();

			return new { baseHc, nestedHc }.GetHashCode();
		}

		#endregion
	}
	[ContentItemName("Repeat")]
	public class RepeatContent : HiddenContent<RepeatContent>, IEquatable<RepeatContent>
	{
        #region properties
	    
	    public ICollection<Content> Items { get; set; }

        public IEnumerable<string> FieldNames
        {
            get
            {
                return Items?.SelectMany(r => r.FieldNames).Distinct().ToList() ?? new List<string>();
            }
        }

        #endregion properties

        #region ctors

        public RepeatContent()
        {
            
        }

        public RepeatContent(string name)
        {
            Name = name;
        }

		public RepeatContent(string name, IEnumerable<Content> items)
            : this(name)
        {
            Items = items.ToList();
        }

		public RepeatContent(string name, params Content[] items)
            : this(name)
        {
			Items = items.ToList();
        }

		#endregion

		#region Fluent

		public static RepeatContent Create(string name, params Content[] items)
        {
			return new RepeatContent(name, items);
        }

		public static RepeatContent Create(string name, IEnumerable<Content> items)
        {
			return new RepeatContent(name, items);
        }

		public RepeatContent AddItem(Content item)
		{
			if (Items == null) Items = new Collection<Content>();
			Items.Add(item);
			return this;
		}

		public RepeatContent AddItem(params IContentItem[] contentItems)
		{
			if (Items == null) Items = new Collection<Content>();
			Items.Add(new Content(contentItems));
			return this;
		}

        #endregion

        #region Equals

        public bool Equals(RepeatContent other)
		{
			if (other == null) return false;
			return Name.Equals(other.Name) &&
			       FieldNames.SequenceEqual(other.FieldNames);
		}

		public override bool Equals(IContentItem other)
		{
			if (!(other is RepeatContent)) return false;

			return Equals((RepeatContent)other);
		}


		public override int GetHashCode()
		{
			var hc = 0;
			if (Items != null)
				hc = Items.Aggregate(hc, (current, p) => current ^ p.GetHashCode());
			
			return new { Name, hc }.GetHashCode();
		}

		#endregion
	}
}