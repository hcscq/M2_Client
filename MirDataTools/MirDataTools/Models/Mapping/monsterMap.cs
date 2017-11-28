using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace MirDataTools.Models.Mapping
{
    public class monsterMap : EntityTypeConfiguration<monster>
    {
        public monsterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.列_0)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.列_1)
                .HasMaxLength(50);

            this.Property(t => t.列_2)
                .HasMaxLength(50);

            this.Property(t => t.列_3)
                .HasMaxLength(50);

            this.Property(t => t.列_4)
                .HasMaxLength(50);

            this.Property(t => t.列_5)
                .HasMaxLength(50);

            this.Property(t => t.列_6)
                .HasMaxLength(50);

            this.Property(t => t.列_7)
                .HasMaxLength(50);

            this.Property(t => t.列_8)
                .HasMaxLength(50);

            this.Property(t => t.列_9)
                .HasMaxLength(50);

            this.Property(t => t.列_10)
                .HasMaxLength(50);

            this.Property(t => t.列_11)
                .HasMaxLength(50);

            this.Property(t => t.列_12)
                .HasMaxLength(50);

            this.Property(t => t.列_13)
                .HasMaxLength(50);

            this.Property(t => t.列_14)
                .HasMaxLength(50);

            this.Property(t => t.列_15)
                .HasMaxLength(50);

            this.Property(t => t.列_16)
                .HasMaxLength(50);

            this.Property(t => t.列_17)
                .HasMaxLength(50);

            this.Property(t => t.列_18)
                .HasMaxLength(50);

            this.Property(t => t.列_19)
                .HasMaxLength(50);

            this.Property(t => t.列_20)
                .HasMaxLength(50);

            this.Property(t => t.列_21)
                .HasMaxLength(50);

            this.Property(t => t.列_22)
                .HasMaxLength(50);

            this.Property(t => t.列_23)
                .HasMaxLength(50);

            this.Property(t => t.列_24)
                .HasMaxLength(50);

            this.Property(t => t.列_25)
                .HasMaxLength(50);

            this.Property(t => t.列_26)
                .HasMaxLength(50);

            this.Property(t => t.newName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("monster");
            this.Property(t => t.列_0).HasColumnName("列 0");
            this.Property(t => t.列_1).HasColumnName("列 1");
            this.Property(t => t.列_2).HasColumnName("列 2");
            this.Property(t => t.列_3).HasColumnName("列 3");
            this.Property(t => t.列_4).HasColumnName("列 4");
            this.Property(t => t.列_5).HasColumnName("列 5");
            this.Property(t => t.列_6).HasColumnName("列 6");
            this.Property(t => t.列_7).HasColumnName("列 7");
            this.Property(t => t.列_8).HasColumnName("列 8");
            this.Property(t => t.列_9).HasColumnName("列 9");
            this.Property(t => t.列_10).HasColumnName("列 10");
            this.Property(t => t.列_11).HasColumnName("列 11");
            this.Property(t => t.列_12).HasColumnName("列 12");
            this.Property(t => t.列_13).HasColumnName("列 13");
            this.Property(t => t.列_14).HasColumnName("列 14");
            this.Property(t => t.列_15).HasColumnName("列 15");
            this.Property(t => t.列_16).HasColumnName("列 16");
            this.Property(t => t.列_17).HasColumnName("列 17");
            this.Property(t => t.列_18).HasColumnName("列 18");
            this.Property(t => t.列_19).HasColumnName("列 19");
            this.Property(t => t.列_20).HasColumnName("列 20");
            this.Property(t => t.列_21).HasColumnName("列 21");
            this.Property(t => t.列_22).HasColumnName("列 22");
            this.Property(t => t.列_23).HasColumnName("列 23");
            this.Property(t => t.列_24).HasColumnName("列 24");
            this.Property(t => t.列_25).HasColumnName("列 25");
            this.Property(t => t.列_26).HasColumnName("列 26");
            this.Property(t => t.newName).HasColumnName("newName");
            this.Property(t => t.Id).HasColumnName("Id");
        }
    }
}
