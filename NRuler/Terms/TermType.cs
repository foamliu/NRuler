using System;

namespace NRuler.Terms
{
    ///<summary>
    /// Indicates the type of term.
    ///</summary>
    [Serializable]
    public enum TermType
    {
        ///<summary>Function
        ///</summary>
        Function = 1,
        ///<summary>Variable
        ///</summary>
        Variable = 2,
        ///<summary>String
        ///</summary>
        String = 3,
        ///<summary>Integer
        ///</summary>
        Integer = 4,
        ///<summary>Double
        ///</summary>
        Double = 5,
        ///<summary>ObjectReference
        ///</summary>
        ObjectReference = 6,
        ///<summary>Boolean
        ///</summary>
        Boolean = 7,
        ///<summary>DateTime
        ///</summary>
        DateTime = 8,
        ///<summary>
        ///</summary>List
        List,
        ///<summary>Null
        ///</summary>
        Null,
        ///<summary>Guid
        ///</summary>
        Guid,
        ///<summary>Collection
        ///</summary>
        Collection,
        ///<summary>ObjectRelation
        ///</summary>
        ObjectRelation,
        ///<summary>EntityObject
        ///</summary>
        EntityObject
    }
}