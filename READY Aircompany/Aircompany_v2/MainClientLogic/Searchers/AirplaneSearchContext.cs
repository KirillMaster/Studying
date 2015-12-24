using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    class AirplaneSearchContext
    {
        LogicPartsKeeper logicParts;
        List<AirplanesWithTypes> airplanesWithTypesList;
        List<Airplane> airplanes;
        List<AirplaneTypeInfo> airplaneTypesInfo;
        List<AirplaneType> airplaneTypes;
        AirportEntities databaseContext;
        List<AirplanesIDWithTypeName> airplanesWithTypeNames;
        public AirplaneSearchContext()
        {
            logicParts = LogicPartsKeeper.getInstance();
            databaseContext = new AirportEntities();
            airplanesWithTypesList = new List<AirplanesWithTypes>();
            airplanes = new List<Airplane>();
            airplaneTypesInfo = new List<AirplaneTypeInfo>();
            databaseContext = new AirportEntities();
            airplanesWithTypeNames = new List<AirplanesIDWithTypeName>();
           
        }
        public List<AirplanesWithTypes> getAirplanesJoinTypes(object sender, EventArgs e)
        {
            SelectAllAirplanesAndAirplaneTypeItems();

            airplanesWithTypesList.Clear();
            foreach (Airplane plane in airplanes)
            {
                foreach (AirplaneTypeInfo type in airplaneTypesInfo)
                {
                    if (plane.airplane_type.Value == type.ID)
                    {
                        airplanesWithTypesList.Add(new AirplanesWithTypes(
                            plane.ID_airplane,
                            type.ID,
                            type.EconomCount,
                            type.BussinessCount,
                            type.CrewCount,
                            type.Carrying,
                            type.TypeName)
                            );
                    }
                }
                
            }
            return airplanesWithTypesList;
           // if (AirplaneWithTypesResponse != null)
           //     AirplaneWithTypesResponse(this, airplanesWithTypesList);
        }
        private void SelectAllAirplanesAndAirplaneTypeItems()
        {
            AirplaneLogic airplaneLogic = logicParts.airplaneLogic;
            AirplaneTypeLogic airplaneTypeLogic = logicParts.airplaneTypeLogic;
            airplaneLogic.SelectAll();
            airplaneTypeLogic.SelectAll();
            airplanes = airplaneLogic.airplanes;
            airplaneTypes = airplaneTypeLogic.airplaneTypeList;
            airplaneTypes.ForEach(type => airplaneTypesInfo.Add(new AirplaneTypeInfo(type.airplane_type,
                type.name_,
                type.carrying,
                type.crew_count,
                type.business_seats_count,
                type.econom_seats_count)));
        }
        private void AirplaneSearchByTypeName(AirplanesIDWithTypeName airplane)
        {
            var res = from plane in databaseContext.Airplane
                      join type in databaseContext.AirplaneType
                      on plane.airplane_type.Value equals type.airplane_type
                      where type.name_ == airplane.name
                      select new { plane.ID_airplane, type.name_};
            foreach (var item in res)
            {
                airplanesWithTypeNames.Add(new AirplanesIDWithTypeName(item.ID_airplane, item.name_));
            }
        }
        public List<AirplaneTypeInfo> AirplaneTypeSearchByTypeName(string name)
        {
            var types = databaseContext.AirplaneType.Select(item => item).Where(item => item.name_.Equals(name));
           // airplaneTypes.Clear();
            foreach (var type in types)
            {
                AirplaneTypeInfo airplaneTypeInfo = new AirplaneTypeInfo(type.airplane_type,
                    type.name_,
                    type.carrying.Value,
                    type.crew_count.Value,
                    type.business_seats_count.Value,
                    type.econom_seats_count.Value);

                airplaneTypesInfo.Add(airplaneTypeInfo);
            }
            return airplaneTypesInfo;
        }
        private List<AirplanesIDWithTypeName> AirplaneSearchByID(AirplanesIDWithTypeName airplane)
        {
            var res = from plane in databaseContext.Airplane
                      join type in databaseContext.AirplaneType
                      on plane.airplane_type.Value equals type.airplane_type
                      where plane.ID_airplane == airplane.ID_airplane
                      select new { plane.ID_airplane, type.name_ };

            foreach (var item in res)
            {

                airplanesWithTypeNames.Add(new AirplanesIDWithTypeName(item.ID_airplane, item.name_));
            }
            return airplanesWithTypeNames;
        }
        public List<AirplanesIDWithTypeName> AirplaneSearch(AirplanesIDWithTypeName airplane)
        {
            //airplanesWithTypeNames.Clear();
            if (airplane.name != null)
            {
                AirplaneSearchByTypeName(airplane);
            }
            else if (airplane.ID_airplane != 0)
            {
                AirplaneSearchByID(airplane);
            }
            return airplanesWithTypeNames;
        }
    }
}
