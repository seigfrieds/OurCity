/// Generative AI - CoPilot was used to assist in the creation of this file.
///   CoPilot was asked to help write unit tests for the components by being given
///   a description of what exactly should be tested for this component and giving
///   back the needed functions and syntax to implement the tests.

import { describe, it, expect } from "vitest";
import { mount } from "@vue/test-utils";
import PrimeVue from "primevue/config";
import CommentInput from "@/components/CommentInput.vue";

global.ResizeObserver = class {
  observe() {}
  unobserve() {}
  disconnect() {}
};

describe("CommentInput", () => {
  it("renders the textarea with placeholder", () => {
    const wrapper = mount(CommentInput, {
      global: {
        plugins: [PrimeVue],
      },
    });
    const textarea = wrapper.find(".comment-input");
    expect(textarea.exists()).toBe(true);
    expect(textarea.attributes("placeholder")).toBe("Add your thoughts...");
  });

  it("emits 'submit comment' event with text when button is clicked", async () => {
    const wrapper = mount(CommentInput, {
      global: {
        plugins: [PrimeVue],
      },
    });
    const textarea = wrapper.find(".comment-input");
    await textarea.setValue("i am testing the comment input");
    await wrapper.vm.$nextTick();
    const submitButton = wrapper.find(".comment-arrow-btn");
    await submitButton.trigger("click");
    expect(wrapper.emitted()).toHaveProperty("submit comment");
    expect(wrapper.emitted()["submit comment"][0]).toEqual(["i am testing the comment input"]);
  });
});
