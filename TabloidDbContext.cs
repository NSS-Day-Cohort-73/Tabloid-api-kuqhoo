using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tabloid.Models;

namespace Tabloid.Data;

public class TabloidDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<PostReaction> PostReactions { get; set; }
    public DbSet<ReactionType> ReactionTypes { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<AdminLog> AdminLogs { get; set; }

    public TabloidDbContext(DbContextOptions<TabloidDbContext> context, IConfiguration config)
        : base(context)
    {
        _configuration = config;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<IdentityRole>()
            .HasData(
                new IdentityRole
                {
                    Id = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                    Name = "Admin",
                    NormalizedName = "admin",
                }
            );

        modelBuilder
            .Entity<IdentityUser>()
            .HasData(
                new IdentityUser[]
                {
                    new IdentityUser
                    {
                        Id = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                        UserName = "Administrator",
                        Email = "admina@strator.comx",
                        PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                            null,
                            _configuration["AdminPassword"]
                        ),
                    },
                    new IdentityUser
                    {
                        Id = "d8d76512-74f1-43bb-b1fd-87d3a8aa36df",
                        UserName = "JohnDoe",
                        Email = "john@doe.comx",
                        PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                            null,
                            _configuration["AdminPassword"]
                        ),
                    },
                    new IdentityUser
                    {
                        Id = "a7d21fac-3b21-454a-a747-075f072d0cf3",
                        UserName = "JaneSmith",
                        Email = "jane@smith.comx",
                        PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                            null,
                            _configuration["AdminPassword"]
                        ),
                    },
                    new IdentityUser
                    {
                        Id = "c806cfae-bda9-47c5-8473-dd52fd056a9b",
                        UserName = "AliceJohnson",
                        Email = "alice@johnson.comx",
                        PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                            null,
                            _configuration["AdminPassword"]
                        ),
                    },
                    new IdentityUser
                    {
                        Id = "9ce89d88-75da-4a80-9b0d-3fe58582b8e2",
                        UserName = "BobWilliams",
                        Email = "bob@williams.comx",
                        PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                            null,
                            _configuration["AdminPassword"]
                        ),
                    },
                    new IdentityUser
                    {
                        Id = "d224a03d-bf0c-4a05-b728-e3521e45d74d",
                        UserName = "EveDavis",
                        Email = "Eve@Davis.comx",
                        PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                            null,
                            _configuration["AdminPassword"]
                        ),
                    },
                }
            );

        modelBuilder
            .Entity<IdentityUserRole<string>>()
            .HasData(
                new IdentityUserRole<string>[]
                {
                    new IdentityUserRole<string>
                    {
                        RoleId = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                        UserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                    },
                    new IdentityUserRole<string>
                    {
                        RoleId = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                        UserId = "d8d76512-74f1-43bb-b1fd-87d3a8aa36df",
                    },
                }
            );
        modelBuilder
            .Entity<UserProfile>()
            .HasData(
                new UserProfile[]
                {
                    new UserProfile
                    {
                        Id = 1,
                        IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                        FirstName = "Admina",
                        LastName = "Strator",
                        ImageLocation =
                            "https://robohash.org/numquamutut.png?size=150x150&set=set1",
                        CreateDateTime = new DateTime(2022, 1, 25),
                    },
                    new UserProfile
                    {
                        Id = 2,
                        FirstName = "John",
                        LastName = "Doe",
                        CreateDateTime = new DateTime(2023, 2, 2),
                        ImageLocation =
                            "https://robohash.org/nisiautemet.png?size=150x150&set=set1",
                        IdentityUserId = "d8d76512-74f1-43bb-b1fd-87d3a8aa36df",
                    },
                    new UserProfile
                    {
                        Id = 3,
                        FirstName = "Jane",
                        LastName = "Smith",
                        CreateDateTime = new DateTime(2022, 3, 15),
                        ImageLocation =
                            "https://robohash.org/molestiaemagnamet.png?size=150x150&set=set1",
                        IdentityUserId = "a7d21fac-3b21-454a-a747-075f072d0cf3",
                    },
                    new UserProfile
                    {
                        Id = 4,
                        FirstName = "Alice",
                        LastName = "Johnson",
                        CreateDateTime = new DateTime(2023, 6, 10),
                        ImageLocation =
                            "https://robohash.org/deseruntutipsum.png?size=150x150&set=set1",
                        IdentityUserId = "c806cfae-bda9-47c5-8473-dd52fd056a9b",
                    },
                    new UserProfile
                    {
                        Id = 5,
                        FirstName = "Bob",
                        LastName = "Williams",
                        CreateDateTime = new DateTime(2023, 5, 15),
                        ImageLocation =
                            "https://robohash.org/quiundedignissimos.png?size=150x150&set=set1",
                        IdentityUserId = "9ce89d88-75da-4a80-9b0d-3fe58582b8e2",
                    },
                    new UserProfile
                    {
                        Id = 6,
                        FirstName = "Eve",
                        LastName = "Davis",
                        CreateDateTime = new DateTime(2022, 10, 18),
                        ImageLocation =
                            "https://robohash.org/hicnihilipsa.png?size=150x150&set=set1",
                        IdentityUserId = "d224a03d-bf0c-4a05-b728-e3521e45d74d",
                    },
                }
            );
        modelBuilder
            .Entity<Category>()
            .HasData(
                new Category { Id = 1, Name = "Technology" },
                new Category { Id = 2, Name = "Travel" },
                new Category { Id = 3, Name = "Lifestyle" },
                new Category { Id = 4, Name = "Programming" },
                new Category { Id = 5, Name = "Food & Cooking" }
            );

        modelBuilder
            .Entity<Tag>()
            .HasData(
                new Tag { Id = 1, Name = "dotnet" },
                new Tag { Id = 2, Name = "web-development" },
                new Tag { Id = 3, Name = "tips" },
                new Tag { Id = 4, Name = "tutorial" },
                new Tag { Id = 5, Name = "beginners" }
            );
        modelBuilder
            .Entity<Post>()
            .HasData(
                new Post
                {
                    Id = 1,
                    UserProfileId = 1,
                    Title = "Getting Started with .NET Identity",
                    Content =
                        "A comprehensive guide to implementing authentication in .NET applications...",
                    CategoryId = 1,
                    IsApproved = true,
                    CreatedAt = new DateTime(2024, 1, 10),
                    HeaderImage = "https://picsum.photos/seed/post1/800/600",
                },
                new Post
                {
                    Id = 2,
                    UserProfileId = 2,
                    Title = "Web Development Best Practices 2024",
                    Content = "Essential best practices for modern web development...",
                    CategoryId = 4,
                    IsApproved = true,
                    CreatedAt = new DateTime(2024, 1, 15),
                    HeaderImage = "https://picsum.photos/seed/post2/800/600",
                },
                new Post
                {
                    Id = 3,
                    UserProfileId = 3,
                    Title = "Modern CSS Techniques",
                    Content = "Exploring modern CSS techniques and best practices...",
                    CategoryId = 4,
                    IsApproved = true,
                    CreatedAt = new DateTime(2024, 1, 20),
                    HeaderImage = "https://picsum.photos/seed/post3/800/600",
                }
            );

        modelBuilder
            .Entity<Comment>()
            .HasData(
                new Comment
                {
                    Id = 1,
                    UserProfileId = 2,
                    PostId = 1,
                    Content = "Stinky!!!!",
                    CreatedAt = new DateTime(2024, 1, 11, 14, 30, 0),
                },
                new Comment
                {
                    Id = 2,
                    UserProfileId = 3,
                    PostId = 1,
                    Content = "This helped me understand authentication better.",
                    CreatedAt = new DateTime(2024, 1, 11, 15, 45, 0),
                },
                new Comment
                {
                    Id = 3,
                    UserProfileId = 4,
                    PostId = 2,
                    Content = "Very useful best practices, thank you.",
                    CreatedAt = new DateTime(2024, 1, 16, 10, 15, 0),
                }
            );

        modelBuilder
            .Entity<PostReaction>()
            .HasData(
                new PostReaction
                {
                    Id = 1,
                    UserProfileId = 2,
                    PostId = 1,
                    reactionTypeId = 1,
                    CreatedAt = new DateTime(2024, 1, 11, 12, 0, 0),
                },
                new PostReaction
                {
                    Id = 2,
                    UserProfileId = 3,
                    PostId = 1,
                    reactionTypeId = 1,
                    CreatedAt = new DateTime(2024, 1, 11, 13, 15, 0),
                },
                new PostReaction
                {
                    Id = 3,
                    UserProfileId = 4,
                    PostId = 2,
                    reactionTypeId = 1,
                    CreatedAt = new DateTime(2024, 1, 16, 14, 30, 0),
                }
            );

        modelBuilder
            .Entity<ReactionType>()
            .HasData(
                new ReactionType
                {
                    Id = 1,
                    Type = "Like",
                    faIcon = "faThumbsUp",
                },
                new ReactionType
                {
                    Id = 2,
                    Type = "Dislike",
                    faIcon = "faThumbsDown",
                },
                new ReactionType
                {
                    Id = 3,
                    Type = "Love",
                    faIcon = "faHeart",
                },
                new ReactionType
                {
                    Id = 4,
                    Type = "Trash",
                    faIcon = "faTrashCan",
                },
                new ReactionType
                {
                    Id = 5,
                    Type = "Angry",
                    faIcon = "faFaceAngry",
                },
                new ReactionType
                {
                    Id = 6,
                    Type = "Sad",
                    faIcon = "faFaceSadCry",
                }
            );

        modelBuilder
            .Entity<Subscription>()
            .HasData(
                new Subscription
                {
                    Id = 1,
                    UserProfileId = 2,
                    AuthorId = 1,
                    SubscribedAt = new DateTime(2024, 1, 15),
                },
                new Subscription
                {
                    Id = 2,
                    UserProfileId = 3,
                    AuthorId = 1,
                    SubscribedAt = new DateTime(2024, 1, 16),
                },
                new Subscription
                {
                    Id = 3,
                    UserProfileId = 4,
                    AuthorId = 2,
                    SubscribedAt = new DateTime(2024, 1, 17),
                }
            );

        // Configure relationships
        modelBuilder
            .Entity<Subscription>()
            .HasOne(s => s.Subscriber)
            .WithMany()
            .HasForeignKey(s => s.UserProfileId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Subscription>()
            .HasOne(s => s.Author)
            .WithMany()
            .HasForeignKey(s => s.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configure Post relationships
        modelBuilder
            .Entity<Post>()
            .HasOne(p => p.UserProfile)
            .WithMany()
            .HasForeignKey(p => p.UserProfileId);

        modelBuilder
            .Entity<Post>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId);
    }
};
