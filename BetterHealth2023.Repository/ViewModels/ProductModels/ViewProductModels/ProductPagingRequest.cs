﻿using BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.ProductModels.ViewProductModels
{
    public class ProductPagingRequest : PagingRequestBase
    {
        /// <summary>
        /// Filter thuốc kê đơn.
        /// </summary>
        public bool? isPrescription { get; set; }
        /// <summary>
        /// Filter thuốc đang bán hay không, chỉ xài được cho nội bộ. Customer Load luôn luôn chỉ load IsSell = true (không thể ghi đè).
        /// Lưu ý: Không nên Filter isSell = false, sẽ không load ra cái nào cả.
        /// </summary>
        public bool? isSell { get; set; }

        /// <summary>
        /// Filter theo đối tượng dùng sản phẩm: null hoặc rỗng (load all), 1 (trẻ em), 2 (người lớn), 3(người cao tuổi), 4 (phụ nữ cho con bú)
        /// </summary>
        [Range(1, 5)]
        public string userTarget { get; set; }
        public string mainCategoryID { get; set; }
        public string subCategoryID { get; set; }
        /// <summary>
        /// Search theo Id, tên sản phẩm, BarCode, Công Dụng và Thành Phần của sản phẩm.
        /// </summary>
        public string productName { get; set; }
        //search theo Name, BarCode
        public string manufacturerID { get; set; }
    }
}
