/// Generative AI - CoPilot was used to assist in the creation of this file.
///   CoPilot was asked to help write unit tests for the components by being given
///   a description of what exactly should be tested for this component and giving
///   back the needed functions and syntax to implement the tests.

import { describe, it, expect } from "vitest";
import { mount } from "@vue/test-utils";
import PostItem from "@/components/PostItem.vue";

describe("PostItem", () => {
  it("renders post details", () => {
    const wrapper = mount(PostItem, {
      props: {
        title: "Test Title",
        location: "Test Location",
        description: "Test Description",
        imageUrl: "https://test.com/testimage.jpg",
      }
    });
    expect(wrapper.text()).toContain("Test Title");
    expect(wrapper.text()).toContain("Test Location");
    expect(wrapper.text()).toContain("Test Description");
    const img = wrapper.find("img");
    expect(img.exists()).toBe(true);
    expect(img.attributes("src")).toBe("https://test.com/testimage.jpg");
  });

  it("emits upvote event when upvote button is clicked", async () => {
    const wrapper = mount(PostItem, {
      props: { 
        title: "Test Title",
        location: "Test Location",
        description: "Test Description",
        imageUrl: "https://test.com/testimage.jpg",
        votes: 0 },
    });
    const upvoteBtn = wrapper.find(".upvote");
    await upvoteBtn.trigger("click");
    expect(wrapper.emitted()).toHaveProperty("upvote");
  });

  it("emits downvote event when downvote button is clicked", async () => {
    const wrapper = mount(PostItem, {
      props: { 
        title: "Test Title",
        location: "Test Location",
        description: "Test Description",
        imageUrl: "https://test.com/testimage.jpg",
        votes: 0 },
    });
    const downvoteBtn = wrapper.find(".downvote");
    await downvoteBtn.trigger("click");
    expect(wrapper.emitted()).toHaveProperty("downvote");
  });
});
