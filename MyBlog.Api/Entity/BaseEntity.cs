using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
        /// <summary>
        /// 添加记录时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新记录时间
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 逻辑删除标记
        /// </summary>
        public bool IsDeleted { get; set; }

    }
}
