using EventPlanner.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Storage.Net;
using Storage.Net.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EventPlanner.Data
{
    public class AttachmentOptions
    {
        public string Root { get; set; }
        public string EventAttachment { get; set; }
        public string SupplierAttachment { get; set; }
    }

    public class AttachmentService
    {
        private readonly IBlobStorage _storage;
        private readonly AppDBContext _context;
        private readonly AttachmentOptions _options;

        public AttachmentService(
            AppDBContext context,
            IConfiguration configuration)
        {
            _context = context;
            _options = configuration.GetSection(nameof(AttachmentOptions)).Get<AttachmentOptions>();
            _storage = StorageFactory.Blobs.DirectoryFiles(_options.Root)
                .WithGzipCompression(System.IO.Compression.CompressionLevel.Optimal);
        }

        public async Task<Stream> GetFile(Attachment attachment)
        {
            Stream stream = new MemoryStream();
            await _storage.ReadToStreamAsync(Path.Combine(attachment.Path, attachment.Name), stream);
            return stream;
        }

        public async Task<byte[]> GetFileByteArray(Attachment attachment)
            => await _storage.ReadBytesAsync(Path.Combine(attachment.Path, attachment.Name));

        public async Task UpdateFiles<T>(Guid objectId, IList<IBrowserFile> files)
            where T : IWithAttachments
        {
            T parent = (T)await _context.FindAsync(typeof(T), objectId);

            string path = "";
            switch (typeof(T))
            {
                case var cls when cls == typeof(Event):
                    {
                        path = _options.EventAttachment;
                        break;
                    }
                case var cls when cls == typeof(Supplier):
                    {
                        path = _options.SupplierAttachment;
                        break;
                    }
                default:
                    throw new NotImplementedException();
            }

            foreach (var f in files)
            {
                var readStream = f.OpenReadStream();
                byte[] bytes = new byte[readStream.Length];
                await readStream.ReadAsync(bytes, 0, (int)readStream.Length);

                string filePath = path.Replace("{parentId}", objectId.ToString());

                string fileName = f.Name;
                while (File.Exists(Path.Combine(_options.Root, filePath, fileName)))
                {
                    var dotIndex = f.Name.LastIndexOf('.');
                    fileName = fileName.Insert(dotIndex, new Random().Next(1, 99).ToString());
                }

                await _storage.WriteAsync(Path.Combine(filePath, fileName), bytes);

                parent.Attachments.Add(new Attachment
                {
                    Name = fileName,
                    Path = filePath
                });
            }

            _context.Update(parent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFile(Attachment attachment)
        {
            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            string fullPath = $"{_options.Root}/{attachment.Path}/{attachment.Name}";
            File.Delete(fullPath);
        }
    }
}
