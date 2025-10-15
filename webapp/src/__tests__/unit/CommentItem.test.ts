/// Generative AI - CoPilot was used to assist in the creation of this file.
///   CoPilot was asked to help write unit tests for the components by being given
///   a description of what exactly should be tested for this component and giving
///   back the needed functions and syntax to implement the tests.

import { describe, it, expect } from "vitest";
import { mount } from "@vue/test-utils";
import CommentItem from "@/components/CommentItem.vue";

describe("CommentItem", () => {
  it("renders author, text, and votes", () => {
    const wrapper = mount(CommentItem, {
      props: {
        id: "1",
        author: "Test Author",
        text: "Test Text",
        votes: 0,
        replies: [],
      },
    });
    expect(wrapper.find(".comment-author").text()).toBe("Test Author");
    expect(wrapper.find(".comment-text").text()).toBe("Test Text");
    expect(wrapper.find(".comment-votes").text()).toBe("0");
  });
});
