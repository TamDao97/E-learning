using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.Combobox
{
    public class ComboboxModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class ComboboxIntModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class ComboboxUnitModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }

    public class ComboboxParentModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
    }

    public class CbbOrderModel
    {
        public int Id { get; set; }
        public int Order { get; set; }
    }

    public class CbbOrderStringModel
    {
        public string Id { get; set; }
        public int Order { get; set; }
    }

    public class ComboboxTopicFullModel
    {
        public string Title { get; set; }
        public string ParentId { get; set; }
        public string Key { get; set; }
        public List<ComboboxTopicFullModel> Children { get; set; }
        public bool IsLeaf { get; set; } = false;
    }
}
