import type { PostProps } from "@/types/interfaces";

const posts: PostProps[] = [
  {
    id: 1,
    author: "a_real_prof",
    title: "A beautiful day in the park",
    location: "St. Vital South",
    description:
      "I spent the afternoon walking through the park, enjoying the beautiful weather. The leaves are just starting to change color, and the air is crisp and cool.",
    votes: 7,
    imageUrls: [
      "https://images.unsplash.com/photo-1503376780353-7e6692767b70?q=80&w=2070&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
    ],
    comments: [],
  },
  {
    id: 2,
    author: "PoStoreGangEyeSayShun",
    title: "Exploring the city streets",
    location: "Tuxedo",
    description:
      "I love the energy of the city. There is always something new to see and do. I could spend hours just walking around and exploring.",
    votes: 21,
    imageUrls: [
      "https://images.unsplash.com/photo-1759167317838-4e77e12621cf?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=744",
      "https://images.unsplash.com/photo-1760422711320-69c99a3fb6f1?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1497",
      "https://images.unsplash.com/photo-1759987651385-342ad67d210d?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=774",
    ],
    comments: [
      {
        id: 1,
        author: "ViewCompOwnItLieBrrrAiry",
        content: "I haven't seen much honestly, where could I go to find some good stuff to do?",
        votes: 12,
        replies: [
          {
            id: 7,
            author: "UnemployedComputerScientist",
            content: "Have you been to the Forks?",
            votes: 5,
            replies: [],
          },
          {
            id: 8,
            author: "Shopoholic",
            content: "theres always stuff to do at the outlet mall",
            votes: 2,
            replies: [],
          },
        ],
      },
      {
        id: 2,
        author: "RealMichaelJordanProbably",
        content: "If you're looking for some recommendations on things to do, I'd highly suggest going to a Sea Bears game.",
        votes: 8,
        replies: [],
      },
      {
        id: 3,
        author: "tacobellnachofries",
        content: "check out Pineridge Hollow! It's a little outside the city but it's so much fun",
        votes: 4,
        replies: [],
      },
      {
        id: 4,
        author: "kingoftouchinggrass",
        content: "bruh did anyone else go to nuit blanche? that was wild",
        votes: 3,
        replies: [],
      },
      {
        id: 5,
        author: "ILoveGambling21",
        content: "the casino is pretty great too",
        votes: 1,
        replies: [],
      },
      {
        id: 6,
        author: "YoungSheldonBazinga",
        content: "I have more fun watching young sheldon at home.",
        votes: 1,
        replies: [],
      },
    ],
  },
  {
    id: 3,
    author: "kraftdinnermaster",
    title: "Can we finish construction around the UofM already?",
    location: "University of Manitoba",
    description: "It takes me so much longer to leave campus with all the construction :(",
    votes: 7,
    imageUrls: [],
    comments: [],
  },
];

export default posts;
