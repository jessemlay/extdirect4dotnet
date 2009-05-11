using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtDirect.Direct
{
    public enum AppliedTo
    {        
        DirectPublic,
        DirectWithinAssembly
        

    }
    public enum MethodVisibility
    {
        Visible,
        Hidden
    
    }
    public enum DirectAction
    {
        
        No,
        Load,
        Create,
        Update,
        Delete,
        Save,
        FormSubmission
        
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DirectServiceAttribute : Attribute
    {
        private string _name;
        private AppliedTo _visibility;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        public AppliedTo Visibility
        {
            get {
                return _visibility;
            }
        }
        public DirectServiceAttribute()
        {
            this._visibility = AppliedTo.DirectPublic;
        }
        public DirectServiceAttribute(string className)
        {
            this._name = className;
            this._visibility = AppliedTo.DirectPublic;

        }
        public DirectServiceAttribute(string className, AppliedTo visibility)
        {
            this._name = className;
            this._visibility = visibility;

        }
    }
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class DirectMethodAttribute : Attribute
    {
        private string _name;
        
        public string MethodName
        {
            get { return _name; }
        }
        private DirectAction _action;
        public DirectAction Action
        {
            get { return _action; }
        }
        private MethodVisibility _visibility;
        public MethodVisibility Visibility
        {
            get { return _visibility; }
        }
        public DirectMethodAttribute()
        {
            this._visibility = MethodVisibility.Visible;
            this._action = DirectAction.No;
        }
        public DirectMethodAttribute(string name)
        {
            this._name = name;
            this._visibility = MethodVisibility.Visible;
            this._action = DirectAction.No;

        }
        public DirectMethodAttribute(string name, DirectAction action)
        {
            this._name = name;
            this._visibility = MethodVisibility.Visible;
            this._action = DirectAction.No;

        }
        public DirectMethodAttribute(string name, DirectAction action,MethodVisibility visibility)
        {
            this._name = name;
            this._visibility = visibility;
            this._action = action;

        }
    }    
}
