using KerbalKonstructs.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KerbalKonstructs.Modules.Career
{
    public class CareerGroups
    {
        private static string groupCFGNodeName = "KKGroups";

        public static void SaveGroups(ConfigNode cfgNode)
        {

            if (cfgNode.HasNode(groupCFGNodeName))
            {
                cfgNode.RemoveNode(groupCFGNodeName);
            }
            ConfigNode buildingNode = cfgNode.AddNode(groupCFGNodeName);

            foreach (GroupCenter center in StaticDatabase.allGroupCenters)
            {
                if (center.isInSavegame)
                {
                    Debug.Log($"Saving group {center.Group} in savegame");
                    ConfigNode instanceNode = buildingNode.AddNode("Group");

                    center.WriteConfig(instanceNode);
                }
            }
        }

        public static void LoadGroups(ConfigNode cfgNode)
        {
            RemoveAllGroups();

            ConfigNode groupNode;
            if (cfgNode.HasNode(groupCFGNodeName))
            {
                groupNode = cfgNode.GetNode(groupCFGNodeName);
            }
            else
            {
                return;
            }


            foreach (ConfigNode instanceNode in groupNode.GetNodes())
            {
                LoadGroup(instanceNode);
            }
        }

        public static void RemoveAllGroups() => StaticDatabase.allGroupCenters.Where(g => g.isInSavegame).ToList().ForEach(g => g.DeleteGroupCenter());

        public static void RemoveBuilding(GroupCenter center)
        {
            if (center.isInSavegame)
            {
                center.DeleteGroupCenter();
            }
        }

        public static void LoadGroup(ConfigNode cfgNode)
        {
            GroupCenter center = new GroupCenter();

            center.isInSavegame = true;

            center.ParseCFGNode(cfgNode);

            center.Spawn();
        }
    }
}
