﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
//	   如存在本生成代码外的新需求，请在相同命名空间下创建同名分部类进行实现。
// </auto-generated>
//
// <copyright file="UserRepository.generated.cs">
//		Copyright(c)2013 Beyon.All rights reserved.
//		CLR版本：4.5.1
//		开发组织：北京博阳世通信息技术有限公司
//		公司网站：http://www.beyondb.com.cn
//		所属工程：BEYON.Domain.Data
//		生成时间：2016-03-01 16:09
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYON.Component.Data;
using BEYON.Component.Data.EF;
using BEYON.Component.Data.EF.Interface;
using BEYON.Domain.Model.App;


namespace BEYON.Domain.Data.Repositories.App.Impl
{
	/// <summary>
    ///   仓储操作层实现——用户信息
    /// </summary>
    public partial class TaxPerOrderRepository : EFRepositoryBase<TaxPerOrder, Int32>, ITaxPerOrderRepository
    {
        public TaxPerOrderRepository(IUnitOfWork unitOfWork)
            : base()
        { }

        public IList<TaxPerOrder> GetTaxPerOrderBySerialNumber(String serialNumber)
        {
            var q = from p in Context.TaxPerOrders.Where(w => w.SerialNumber == serialNumber)
                    select p;
            return q.ToList();
        }

        public Double GetPayTaxAmount(String certificateID, String taxOrNot) {
            //根据证件号码查询已发放总金额
            //若taxOrNot=='含税'，即sum(select amountY from TaxPerOrders where CertificateID = certificateID)
            //若taxOrNot=='不含税'，即sum(select amountX from TaxPerOrders where CertificateID = certificateID)
            Double amount = 0.0;          
            if (taxOrNot.Equals("含税"))
            { 
                //amount = (from p in Context.TaxPerOrders.Where(w => w.CertificateID == certificateID)
                //            select p.AmountY).Sum();

                var amountTemp = (from p in Context.TaxPerOrders.Where(w => w.CertificateID == certificateID && w.AmountY != null)
                                                  group p by p.CertificateID into g
                                                  select new
                                                  {
                                                      TotalAmountY = g.Sum(t => t.AmountY)
                                                  }).SingleOrDefault();
                if (amountTemp != null)
                {
                    amount = amountTemp.TotalAmountY;
                }
            }
            else if (taxOrNot.Equals("不含税"))
            {
                var amountTemp = (from p in Context.TaxPerOrders.Where(w => w.CertificateID == certificateID && w.AmountX != null)
                                                    group  p  by p.CertificateID into g
                                                        select new
                                                        {
                                                            TotalAmountX = g.Sum(t=>t.AmountX)
                                                        }).SingleOrDefault();
                if (amountTemp != null)
                {
                    amount = amountTemp.TotalAmountX;
                }
            }
            return amount;
        }
        //都按税前加和
        public Double GetPayTaxAmountY(String certificateID)
        {
            //根据证件号码查询已发放总金额
            //若taxOrNot=='含税'，即sum(select amountY from TaxPerOrders where CertificateID = certificateID)
            //若taxOrNot=='不含税'，即sum(select amountX from TaxPerOrders where CertificateID = certificateID)
            Double amount = 0.0;
          
                var amountTemp = (from p in Context.TaxPerOrders.Where(w => w.CertificateID == certificateID && w.AmountY != null)
                                  group p by p.CertificateID into g
                                  select new
                                  {
                                      TotalAmountY = g.Sum(t => t.AmountY)
                                  }).SingleOrDefault();
                if (amountTemp != null)
                {
                    amount = amountTemp.TotalAmountY;
                }
           
            return amount;
        }

        public Double GetDeductTaxSum(String certificateID) {
            Double amount = 0.0;          
            //amount = (from p in Context.TaxPerOrders.Where(w => w.CertificateID == certificateID)
            //          select p).Sum(g => g.Tax);

            var amountTemp = (from p in Context.TaxPerOrders.Where(w => w.CertificateID == certificateID && w.Tax != null)
                              group p by p.CertificateID into g
                              select new
                              {
                                  TotalTax = g.Sum(t => t.Tax)
                              }).SingleOrDefault();
            if (amountTemp != null)
            {
                amount = amountTemp.TotalTax;
            }

            return amount;
        }

        public int GetCashCount(String certificateID)
        {
            int count = 0;
            var temp = from p in Context.TaxPerOrders.Where(w => w.CertificateID == certificateID)
                       select p;
            if (temp != null) {
                List<TaxPerOrder> list = temp.ToList();
                for (int i = 0; i < list.Count;  i++) {
                    if (list[i].PaymentType.Equals("现金支付")) {
                        count++;
                    }
                }
            }
            return count;
        }

     }
}
