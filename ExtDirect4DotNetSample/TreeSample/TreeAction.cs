using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ExtDirect4DotNet;
using System.Collections.Generic;
using ExtDirect4DotNet.interfaces;
using System.Web.SessionState;
using ExtDirect4DotNet.exceptions;

namespace WebApplication1.TreeSample
{
    public class TreeNode
    {
        public string id
        {
            get;
            set;
        }
        public string text
        {
            get;
            set;
        }
        public bool leaf
        {
            get;
            set;
        }

        public List<TreeNode> children
        {
            get;
            set;
        }

        public TreeNode getChildById(string id) {
            foreach(TreeNode node in this.children) {
                if(node.id == id) 
                    return node;

                if(node.leaf==false) {
                    TreeNode childsChild = node.getChildById(id);
                    if(childsChild != null)
                        return childsChild;
                }
            }
            return null;
        }
        
    }

    [DirectAction]
    public class TreeAction : IActionWithAfterCreation<HttpContext>
    {
        [DirectMethod(MethodType=DirectMethodType.TreeLoad)]
        public List<TreeNode> getChildNodes(string id)
        {
            TreeNode rootNode = this.getData();

            if(rootNode.id == id) {
                return rootNode.children;
            }

            TreeNode found = rootNode.getChildById(id);
            if (found == null)
            {
                throw new DirectException("No TreeNode with the id: "+ id +" found");
            }
            return found.children;
        }

        /// <summary>
        /// Returns a (cached) RootNode from the Session
        /// </summary>
        /// <returns>The RootNode</returns>
        private TreeNode getData()
        {
            return getData(false);
        }

        /// <summary>
        /// Returns a (cached) RootNode from the Session
        /// </summary>
        /// <param name="fresh">true to clear the Cache</param>
        /// <returns>The RootNode</returns>
        private TreeNode getData(Boolean fresh)
        {

            TreeNode rootNode = (TreeNode)Session["TreeActionData"];
            if (rootNode == null || fresh)
            {

                
                TreeNode child1_child1, child1_child2, child1_child3, child1_child4, child2_child5, child2_child6;

                child2_child6 = new TreeNode() {text = "Childs Child No. 6", leaf=true,id="1/2/6"};
                child2_child5 = new TreeNode() {text = "Childs Child No. 5", leaf=true,id="1/2/5"};
                child1_child4 = new TreeNode() {text = "Childs Child No. 4", leaf=true,id="1/2/4"};
                child1_child3 = new TreeNode() {text = "Childs Child No. 3", leaf=true,id="1/1/3"};
                child1_child2 = new TreeNode() {text = "Childs Child No. 2", leaf=true,id="1/1/2"};
                child1_child1 = new TreeNode() {text = "Childs Child", leaf=true,id="1/1/1"};


                List<TreeNode> child_children1 = new List<TreeNode>();
                child_children1.Add(child1_child1);
                child_children1.Add(child1_child2);
                child_children1.Add(child1_child3);
                child_children1.Add(child1_child4);

                List<TreeNode> child_children2 = new List<TreeNode>();
                child_children2.Add(child2_child5);
                child_children2.Add(child2_child6);

                TreeNode child1, child2, child3, child4, child5, child6;

                child6 = new TreeNode() {text = "Node No. 6", leaf=true,id="1/6"};
                child5 = new TreeNode() {text = "Node No. 5", leaf=true,id="1/5"};
                child4 = new TreeNode() {text = "Node No. 4", leaf=true,id="1/4"};
                child3 = new TreeNode() {text = "Node No. 3", leaf=true,id="1/3"};
                child2 = new TreeNode() {text = "Node No. 2", leaf=false,id="1/2", children=child_children2};
                child1 = new TreeNode() {text = "FirstNode", leaf=false,id="1/1", children=child_children1};
                                
                List<TreeNode> children = new List<TreeNode>();
                children.Add(child1);
                children.Add(child2);
                children.Add(child3);
                children.Add(child4);
                children.Add(child5);
                children.Add(child6);

                rootNode = new TreeNode(){text = "Root - Node", leaf=false,id="1", children=children};

                Session["TreeActionData"] = rootNode;

            }

            return rootNode;
        }

        #region IActionWithAfterCreation<HttpContext> Member
        private HttpSessionState Session;
        public void afterCreation(HttpContext parameter)
        {
            Session = parameter.Session;
        }

        #endregion
    }
}
