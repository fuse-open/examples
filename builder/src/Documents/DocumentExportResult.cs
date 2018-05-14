using System;
using System.Collections.Generic;

namespace Builder.Documents
{
    public class DocumentExportResult
    {
        public bool IsExternal { get; }
        public int Slot { get; }
        public string Id { get; }
        public string Title { get; }
        public string PreviewImage { get; }
        public string DetailedPreviewImage { get; }
        public string Synopsis { get; }
        public string Uri { get; }
        public DateTime PublishedAt { get; }
        public List<string> Tags { get; }

        public DocumentExportResult(bool isExternal,
                                    int slot,
                                    string id,
                                    string title,
                                    string synopsis,
                                    string previewImage,
                                    string detailedPreviewImage,
                                    string uri,
                                    DateTime publishedAt,
                                    List<string> tags)
        {
            IsExternal = isExternal;
            Slot = slot;
            Id = id;
            Title = title;
            Synopsis = synopsis;
            PreviewImage = previewImage;
            DetailedPreviewImage = detailedPreviewImage;
            Uri = uri;
            PublishedAt = publishedAt;
            Tags = tags;
        }
    }
}