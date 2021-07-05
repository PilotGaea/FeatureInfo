using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PilotGaea.Geometry;
using PilotGaea.TMPEngine;
using PilotGaea.Serialize;

namespace FeatureInfo
{
    public class FeatureInfoClass : FeatureInfoBaseClass
    {
        public override void DeInit()
        {
        }

        public override bool GetFeatureInfo(CGeoDatabase DB, string SessionID, string LayerName, int EPSG, GeoPoint Point, double Distance, out VarStruct Ans)
        {
            bool Ret = false;
            Ans = new VarStruct();
            CVectorLayer Layer = DB.FindLayer(LayerName) as CVectorLayer_Base;
            if (Layer == null) 
                return Ret;
            CEntityFetcher Fetcher = Layer.SearchByDistance(EPSG, Point, Distance);
            VarArray Arr = Ans["features"].CreateArray(Fetcher.Count);
            for (int i = 0; i < Fetcher.Count; i++)
            {
                GeoPoint p = new GeoPoint();
                GeoPolyline pl = new GeoPolyline();
                GeoPolygonSet pgs = new GeoPolygonSet();
                VarStruct feature = Arr[i].CreateStruct();
                switch (Fetcher.GetType(i))
                {
                    case GEO_TYPE.POINT:
                        {
                            Fetcher.GetGeo(i, ref p);
                            feature["geometry"].Set(p);
                        }
                        break;
                    case GEO_TYPE.POLYLINE:
                        {
                            Fetcher.GetGeo(i, ref pl);
                            feature["geometry"].Set(pl);
                        }
                        break;
                    case GEO_TYPE.POLYGONSET:
                        {
                            Fetcher.GetGeo(i, ref pgs);
                            feature["geometry"].Set(pgs);
                        }
                        break;
                }
            }
            Ret = true;
            return Ret;
        }

        public override bool Init()
        {
            return true;
        }
    }
}
