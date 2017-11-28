using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace MirDataTools.Models.Mapping
{
    public class stditemMap : EntityTypeConfiguration<stditem>
    {
        public stditemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.列_0)
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

            this.Property(t => t.列_27)
                .HasMaxLength(50);

            this.Property(t => t.列_28)
                .HasMaxLength(50);

            this.Property(t => t.列_29)
                .HasMaxLength(50);

            this.Property(t => t.列_30)
                .HasMaxLength(50);

            this.Property(t => t.列_31)
                .HasMaxLength(50);

            this.Property(t => t.列_32)
                .HasMaxLength(50);

            this.Property(t => t.列_33)
                .HasMaxLength(50);

            this.Property(t => t.列_34)
                .HasMaxLength(50);

            this.Property(t => t.列_35)
                .HasMaxLength(50);

            this.Property(t => t.列_36)
                .HasMaxLength(50);

            this.Property(t => t.列_37)
                .HasMaxLength(50);

            this.Property(t => t.列_38)
                .HasMaxLength(50);

            this.Property(t => t.列_39)
                .HasMaxLength(50);

            this.Property(t => t.列_40)
                .HasMaxLength(50);

            this.Property(t => t.列_41)
                .HasMaxLength(50);

            this.Property(t => t.列_42)
                .HasMaxLength(50);

            this.Property(t => t.列_43)
                .HasMaxLength(50);

            this.Property(t => t.列_44)
                .HasMaxLength(50);

            this.Property(t => t.列_45)
                .HasMaxLength(50);

            this.Property(t => t.列_46)
                .HasMaxLength(50);

            this.Property(t => t.列_47)
                .HasMaxLength(50);

            this.Property(t => t.列_48)
                .HasMaxLength(50);

            this.Property(t => t.列_49)
                .HasMaxLength(50);

            this.Property(t => t.newName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("stditems");
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
            this.Property(t => t.列_27).HasColumnName("列 27");
            this.Property(t => t.列_28).HasColumnName("列 28");
            this.Property(t => t.列_29).HasColumnName("列 29");
            this.Property(t => t.列_30).HasColumnName("列 30");
            this.Property(t => t.列_31).HasColumnName("列 31");
            this.Property(t => t.列_32).HasColumnName("列 32");
            this.Property(t => t.列_33).HasColumnName("列 33");
            this.Property(t => t.列_34).HasColumnName("列 34");
            this.Property(t => t.列_35).HasColumnName("列 35");
            this.Property(t => t.列_36).HasColumnName("列 36");
            this.Property(t => t.列_37).HasColumnName("列 37");
            this.Property(t => t.列_38).HasColumnName("列 38");
            this.Property(t => t.列_39).HasColumnName("列 39");
            this.Property(t => t.列_40).HasColumnName("列 40");
            this.Property(t => t.列_41).HasColumnName("列 41");
            this.Property(t => t.列_42).HasColumnName("列 42");
            this.Property(t => t.列_43).HasColumnName("列 43");
            this.Property(t => t.列_44).HasColumnName("列 44");
            this.Property(t => t.列_45).HasColumnName("列 45");
            this.Property(t => t.列_46).HasColumnName("列 46");
            this.Property(t => t.列_47).HasColumnName("列 47");
            this.Property(t => t.列_48).HasColumnName("列 48");
            this.Property(t => t.列_49).HasColumnName("列 49");
            this.Property(t => t.newName).HasColumnName("newName");
            this.Property(t => t.Id).HasColumnName("Id");
        }
    }
}
