﻿using EventPlanner.Models;
using Microsoft.AspNetCore.Components.Forms;
using Storage.Net;
using Storage.Net.Blobs;

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

        public async Task UpdateFiles<T>(Guid objectId, IList<IBrowserFile> files)
            where T : IWithAttachments
        {
            T parent = (T)await _context.FindAsync(typeof(T), objectId);

            foreach (var f in files)
            {
                var readStream = f.OpenReadStream();
                byte[] bytes = new byte[readStream.Length];
                await readStream.ReadAsync(bytes, 0, (int)readStream.Length);
                string filePath = Path.Combine(_options.Root, _options.EventAttachment.Replace("{parentId}", objectId.ToString()));
                await _storage.WriteAsync(Path.Combine(filePath, f.Name), bytes);

                parent.Attachments.Add(new Attachment
                {
                    Name = f.Name,
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
            File.Delete(Path.Combine(attachment.Path, attachment.Name));
        }
    }
}
