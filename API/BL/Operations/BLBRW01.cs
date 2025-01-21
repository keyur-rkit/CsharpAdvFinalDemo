using API.BL.Interface;
using API.Models;
using API.Models.DTO;
using API.Models.Enum;
using API.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.BL.Operations
{
    public class BLBRW01 : IDataHandler<DTOBRW01>
    {
        private BRW01 _objBRW01;

        public EnmType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void PreSave(DTOBRW01 objDTO)
        {
            throw new NotImplementedException();
        }

        public Response Save()
        {
            throw new NotImplementedException();
        }

        public Response Validation()
        {
            throw new NotImplementedException();
        }
    }
}