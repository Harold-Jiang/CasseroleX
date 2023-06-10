using CasseroleX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;
public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> b)
    {
        b.HasKey(s => s.Id); 
        b.Property(e => e.Id)
            .HasComment("ID");
        b.Property(e => e.UserId)
            .HasComment("用户ID");
        b.Property(e => e.Category)
            .HasMaxLength(50)
            .HasDefaultValueSql("''")
            .HasComment("类别");
        b.Property(e => e.ExtParam)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("透传数据");
        b.Property(e => e.FileName)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("文件名称");
        b.Property(e => e.FileSize)
            .HasComment("文件大小");
        b.Property(e => e.ImageFrames)
            .HasComment("图片帧数");
        b.Property(e => e.ImageHeight) 
            .HasComment("高度");
        b.Property(e => e.ImageType)
            .HasMaxLength(30)
            .HasDefaultValueSql("''")
            .HasComment("图片类型");
        b.Property(e => e.ImageWidth) 
            .HasComment("宽度");
        b.Property(e => e.MimeType)
            .HasMaxLength(100)
            .HasDefaultValueSql("''")
            .HasComment("mime类型");
        b.Property(e => e.Sha1)
            .HasMaxLength(40)
            .HasDefaultValueSql("''")
            .HasComment("文件 sha1编码");
        b.Property(e => e.Storage)
            .HasMaxLength(100)
            .HasDefaultValueSql("'local'")
            .HasComment("存储位置");
        b.Property(e => e.LastModified)
            .HasComment("更新时间");
        b.Property(e => e.UploadTime)
            .HasComment("上传时间");
        b.Property(e => e.Url)
            .HasMaxLength(255)
            .HasDefaultValueSql("''")
            .HasComment("物理路径") ; 
        b.Property(e => e.CreateTime)
            .HasComment("创建时间")
            .HasDefaultValue(DateTime.Now); 
    }
}
