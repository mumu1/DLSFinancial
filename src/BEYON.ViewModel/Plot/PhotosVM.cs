using System;
using System.ComponentModel.DataAnnotations;

namespace BEYON.ViewModel.Plot
{
    public class PhotosVM
    {
        public PhotosVM()
        {

        }
        [Required]
        [Display(Name = "遗址编号")]
        [StringLength(36)]
        public string UmrID { get; set; }

        [Display(Name = "图片编号")]
        [StringLength(36)]
        public string P_ID { get; set; }

        [Display(Name = "序号")]
        [StringLength(12)]
        public string Counter { get; set; }

        [Display(Name = "图片名称")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "用户编号")]
        [StringLength(20)]
        public string UserID { get; set; }

        [Display(Name = "底片号")]
        [StringLength(20)]
        public string NegativeID { get; set; }

        [Display(Name = "照片号")]
        [StringLength(25)]
        public string PhotoID { get; set; }

        [Display(Name = "摄影者")]
        [StringLength(25)]
        public string Cameraman { get; set; }

        [Display(Name = "拍摄时间")]
        public string Time { get; set; }

        [Display(Name = "拍摄方向")]
        [StringLength(25)]
        public string Orientation { get; set; }

        [Display(Name = "备注")]
        [StringLength(512)]
        public string Remark { get; set; }

        [Display(Name = "照片文件路径")]
        [StringLength(200)]
        public string FilePath { get; set; }

        [Display(Name = "后缀")]
        [StringLength(50)]
        public string Suffix { get; set; }

        [Display(Name = "图片")]
        public byte[] Annex { get; set; }

        [Display(Name = "审核状态")]
        [StringLength(50)]
        public string Audit { get; set; }
        [Display(Name = "用户角色")]
        [StringLength(50)]
        public string RoleId { get; set; }
        [Display(Name = "修改内容")]
        [StringLength(300)]
        public string Edit { get; set; }
        [Display(Name = "当前模块")]
        [StringLength(20)]
        public string CurrentModel { get; set; }
    }
}
