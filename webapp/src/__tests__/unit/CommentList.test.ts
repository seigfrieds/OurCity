/// Generative AI - CoPilot was used to assist in the creation of this file.
///   CoPilot was asked to help write unit tests for the components by being given
///   a description of what exactly should be tested for this component and giving
///   back the needed functions and syntax to implement the tests.

import { describe, it, expect } from "vitest";
import { mount } from "@vue/test-utils";
import CommentList from "@/components/CommentList.vue";

describe("CommentList", () => {
  it("renders a list of comments", () => {
    const comments = [
      { 
        id: 1, 
        author: "Test Author 1", 
        text: "Test Text 1", 
        votes: 2, 
        replies: [] 
      },
      { 
        id: 2, 
        author: "Test Author 2", 
        text: "Test Text 2", 
        votes: 3, 
        replies: [] 
      }
    ];
    const wrapper = mount(CommentList, {
      props: { comments },
      global: {
        stubs: {
          CommentItem: {
            props: ['author', 'text'],
            template: "<div class='stub-comment-item'>{{ author }}: {{ text }}</div>"
          }
        }
      }
    });
    const commentItems = wrapper.findAll(".stub-comment-item");
    expect(commentItems.length).toBe(2);
    expect(wrapper.text()).toContain("Test Author 1");
    expect(wrapper.text()).toContain("Test Text 1");
    expect(wrapper.text()).toContain("Test Author 2");
    expect(wrapper.text()).toContain("Test Text 2");
  });
});
