using RecipeBookMvc.Repositories.Abstract;

namespace RecipeBookMvc.Repositories.Implementation
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment environment;
        public FileService(IWebHostEnvironment env)
        {
            this.environment = env;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            if (imageFile == null)
            {
                return new Tuple<int, string>(0, "No image file provided");
            }

            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }

                string uniqueString = Guid.NewGuid().ToString();

                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);

                using (var stream = new FileStream(fileWithPath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                return new Tuple<int, string>(1, newFileName);
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, "Error has occurred");
            }
        }


        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = this.environment.WebRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
