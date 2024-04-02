namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            // DbInitializer.ResetDatabase(context);


            //Console.WriteLine(ExportAlbumsInfo(context, 9));
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumInfo = context.Producers
                .Include(x => x.Albums).ThenInclude(a => a.Songs).ThenInclude(s => s.Writer)
                .First(x => x.Id == producerId)
                .Albums.Select(x => new
                {
                    AlbumName = x.Name,
                    ReleaseDate = x.ReleaseDate,
                    ProducerName = x.Producer.Name,
                    AlbumsSongs = x.Songs.Select(x => new
                    {
                        SongName = x.Name,
                        SongPrice = x.Price,
                        SongsWriterName = x.Writer.Name
                    }).OrderByDescending(x => x.SongName)
                      .ThenBy(x => x.SongsWriterName),
                    AlbumPrice = x.Price

                }).OrderByDescending(x => x.AlbumPrice).AsEnumerable();

            StringBuilder sb = new StringBuilder();

            foreach (var album in albumInfo)
            {
                sb
                 .AppendLine($"-AlbumName: {album.AlbumName}")
                 .AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy")}")
                 .AppendLine($"-ProducerName: {album.ProducerName}")
                 .AppendLine($"-Songs:");


                int counter = 1;
                foreach (var song in album.AlbumsSongs)
                {

                    sb
                       .AppendLine($"---#{counter++}")
                      .AppendLine($"---SongName: {song.SongName}")
                      .AppendLine($"---Price: {song.SongPrice:f2}")
                      .AppendLine($"---Writer: {song.SongsWriterName}");
                }


                sb
                  .AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }



        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Include(s => s.SongPerformers)
                    .ThenInclude(sp => sp.Performer)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                    .ThenInclude(a => a.Producer)
                .AsEnumerable() //.ToList(), .ToArray() трябва да се материализира
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    s.Name,
                    Performers = s.SongPerformers
                        .Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName)
                        .ToList(),
                    WriterName = s.Writer.Name,
                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c"),

                })
                .OrderBy(s => s.Name)
                    .ThenBy(s => s.WriterName)
                .ToList();



            StringBuilder stringBuilder = new StringBuilder();
            int counter = 1;
            foreach (var song in songs)
            {

                stringBuilder
                    .AppendLine($"-Song #{counter++}")
                    .AppendLine($"---SongName: {song.Name}")
                    .AppendLine($"---Writer: {song.WriterName}");
                foreach (var performer in song.Performers)
                {
                    stringBuilder.
                        AppendLine($"---Performer: {performer}");
                }
                stringBuilder
                    .AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.Duration}");


            }

            return stringBuilder.ToString().TrimEnd();
        }
    }
}
