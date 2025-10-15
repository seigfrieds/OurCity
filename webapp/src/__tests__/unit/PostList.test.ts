/// Generative AI - CoPilot was used to assist in the creation of this file.
///   CoPilot was asked to help write unit tests for the components by being given
///   a description of what exactly should be tested for this component and giving
///   back the needed functions and syntax to implement the tests.

import { describe, it, expect } from "vitest";
import { mount } from "@vue/test-utils";
import PostList from "@/components/PostList.vue";

describe("PostList", () => {
  it("renders a list of posts", () => {
    const posts = [
      { 
        id: 1, 
        title: "Test Post 1", 
        location: "Test Location 1", 
        description: "Test Description 1", 
        imageUrl: "TestImage.jpg" 
      },
      { 
        id: 2, 
        title: "Test Post 2", 
        location: "Test Location 2", 
        description: "Description 2", 
        imageUrl: "TestImage.jpg" }
    ];
    const wrapper = mount(PostList, {
      props: { posts },
      global: {
        stubs: {
          PostItem: {
            props: ['title'],
            template: "<div class='stub-post-item'>{{ title }}</div>"
          },
          RouterLink: {
            template: "<a><slot /></a>"
          }
        }
      }
    });
    const postItems = wrapper.findAll(".stub-post-item");
    expect(postItems.length).toBe(2);
    expect(wrapper.text()).toContain("Test Post 1");
    expect(wrapper.text()).toContain("Test Post 2");
  });
});
